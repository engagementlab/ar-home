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
    public VideoPlayer videoSource;
    public GameObject videoPlayer;

    private bool tracked;
    private Billboard billboard;
    
    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        
        videoPlayer.SetActive(false);
        
        billboard = videoPlayer.gameObject.AddComponent<Billboard>();
        billboard.PivotAxis = PivotAxis.Y;
        billboard.enabled = false;
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
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    protected virtual void OnTrackingFound()
    {
        if(tracked) return;
        tracked = true;
        
        // Show video component
        Debug.Log("image found");

        billboard.enabled = true;
        videoPlayer.SetActive(true);
        //videoSource.Play();
        
        //Vector3 tempPos = videoPlayer.transform.position;
//        videoPlayer.transform.parent = mTrackableBehaviour.transform;
//        videoPlayer.transform.localPosition = Vector3.zero;
//        videoPlayer.transform.position= mTrackableBehaviour.transform.position;
//        videoPlayer.transform.Translate(Vector3.up * .04f);
//        videoPlayer.transform.position = Vector3.zero;
        //I videoPlayer.transform.position = tempPos;
        
    }


    protected void OnTrackingLost()
    {
        
    }

    #endregion // PRIVATE_METHODS
}
