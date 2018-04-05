/*==============================================================================
Copyright (c) 2018 Engagement Lab @ Emerson College. All Rights Reserved.
by Johnny Richardson
==============================================================================*/

using HoloToolkit.Unity;
using UnityEngine;
using UnityEngine.Video;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class MedallionEventTracker : MonoBehaviour, ITrackableEventHandler
{
    #region PUBLIC_MEMBERS

    public VideoClip clipToPlay;
    public GameObject queueObject;
    public GameObject storyObject;
    public GameObject headPlacementPlaceholderObject;
    public enum TurnOffRendering{
        PlayModeAndDevice,
        PlayModeOnly,
        Neither
    }

    public TurnOffRendering turnOffRendering = TurnOffRendering.PlayModeAndDevice;
    
    #endregion //PUBLIC_MEMBERS

    private bool tracked;
    private Camera mainCamera;
    private VideoPlayer videoSource;
    private Quaternion initalRotation;
    private Transform queueObjectPositioner;
    private Transform storyObjectPositioner;
    
    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        
        // Add video player
        videoSource = gameObject.AddComponent<VideoPlayer>();
        videoSource.playOnAwake = false;
        videoSource.renderMode = VideoRenderMode.RenderTexture;
        videoSource.targetTexture = Resources.Load<RenderTexture>("VideoTex");
        videoSource.clip = clipToPlay;

        mainCamera = Camera.main;
        initalRotation = transform.rotation;
 
        // Find positioner objects
        queueObjectPositioner = transform.Find("Content/QueueObjectPositioner");
        storyObjectPositioner = transform.Find("Content/StoryObjectPositioner");
        
        TurnOffImage();
        
        Events.instance.AddListener<GenericEvent>(PlaceAtAnchor);

    }

    private void OnDestroy()
    {
        Events.instance.RemoveListener<GenericEvent>(PlaceAtAnchor);
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    /// 
    
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    // Disable image target rendering
    private void TurnOffImage()
    {
        if (VuforiaRuntimeUtilities.IsVuforiaEnabled() && 
             turnOffRendering != TurnOffRendering.Neither &&
             (turnOffRendering == TurnOffRendering.PlayModeAndDevice ||
              Application.isEditor))
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            MeshFilter meshFilter = GetComponent<MeshFilter>();

            if (meshRenderer)
                Destroy(meshRenderer);
            if (meshFilter)
                Destroy(meshFilter);
        }
    }

    protected virtual void OnTrackingFound()
    {
        if(tracked) return;
        tracked = true;
        
        // Spawn queue object as child of the queue positioner transform
//        Debug.Log(queueObjectPositioner.position);
        GameObject queueObjectInstance = Instantiate(queueObject, queueObjectPositioner.position, Quaternion.identity);
//        Debug.Log(queueObjectInstance.transform.position);
        GetComponent<ImageTargetBehaviour>().enabled = false;
        
        Debug.Log("Medallion found!");

        // Move back to init rotation
        transform.rotation = initalRotation;
    }

    // Place medallion parent (this gameobject) where player places friend's head in world
    private void PlaceAtAnchor(GenericEvent evt)
    {
       
        if (evt.EventName != "PlaceHead") return;

        GameObject placeholder = GameObject.FindGameObjectWithTag("HeadPlaceholder");
        transform.position = placeholder.transform.position;
        
        // Adopted from billboard script
        Vector3 directionToTarget = mainCamera.transform.position - gameObject.transform.position;
        directionToTarget.z = gameObject.transform.position.z;
        transform.Rotate(new Vector3(0, Quaternion.LookRotation(directionToTarget).y, 0), Space.World);
        
        GameObject storyObjectInstance = Instantiate(storyObject, storyObjectPositioner, false);
        
        Destroy(placeholder);
        
        GetComponent<VideoLogic>().StartVideo();
        
    }

    #endregion // PRIVATE_METHODS
}
