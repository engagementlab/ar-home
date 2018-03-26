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
    
    public VideoPlayer videoSource;
    public GameObject videoPlayer;
    public GameObject queueObject;
    public enum TurnOffRendering{
        PlayModeAndDevice,
        PlayModeOnly,
        Neither
    }

    public TurnOffRendering turnOffRendering = TurnOffRendering.PlayModeAndDevice;
    
    #endregion //PUBLIC_MEMBERS

    private bool tracked;
    private Billboard billboard;
    private Camera mainCamera;
    
    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        
//        videoPlayer.SetActive(false);
//        #if !UNITY_EDITOR    
            queueObject.SetActive(false);        
//        #endif
        
        billboard = videoPlayer.gameObject.AddComponent<Billboard>();
        billboard.PivotAxis = PivotAxis.Y;
        billboard.enabled = false;

        mainCamera = Camera.main;

        TurnOffImage();

    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
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
        Vector3 playerPos = mainCamera.transform.position;
        Vector3 playerDirection = mainCamera.transform.forward;
        
        billboard.enabled = true;
        videoPlayer.SetActive(true);
        videoPlayer.transform.parent = null;            
        
        queueObject.SetActive(true);
        queueObject.transform.position = playerPos + (playerDirection * 1.5f);


    }

    #endregion // PRIVATE_METHODS
}
