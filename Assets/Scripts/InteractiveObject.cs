// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using HoloToolkit.Unity.InputModule;

#if UNITY_WSA || UNITY_STANDALONE_WIN
using UnityEngine.Windows.Speech;
#endif

namespace HoloToolkit.Examples.InteractiveElements
{
    /// <summary>
    /// Interactive exposes basic button type events to the Unity Editor and receives messages from the GestureManager and GazeManager.
    /// 
    /// Beyond the basic button functionality, Interactive also maintains the notion of selection and enabled, which allow for more robust UI features.
    /// InteractiveEffects are behaviors that listen for updates from Interactive, which allows for visual feedback to be customized and placed on
    /// individual elements of the Interactive GameObject
    /// </summary>
    public class InteractiveObject : MonoBehaviour, IInputClickHandler, IFocusable, IInputHandler
    {
        
        /// <summary>
        /// Should the button listen to input?
        /// </summary>
        public bool IsEnabled = true;
        public bool isForPositioning = true;

        /// <summary>
        /// Does the GameObject currently have focus?
        /// </summary>
        public bool HasGaze { get; protected set; }

        /// <summary>
        /// Is the Tap currently in the down state?
        /// </summary>
        public bool HasDown { get; protected set; }

        /// <summary>
        /// Should the button care about click and hold?
        /// </summary>
        public bool DetectHold = false;

        /// <summary>
        /// Configure the amount to time for the hold event to fire
        /// </summary>
        public float HoldTime = 0.5f;

        /// <summary>
        /// Configure the amount of time a roll off update should incur. When building more advanced UI,
        /// we may need to evaluate what the next gazed item is before updating.
        /// </summary>
        public float RollOffTime = 0.02f;

        /// <summary>
        /// Current selected state, can be set from the Unity Editor for default behavior
        /// </summary>
        public bool IsSelected { get; protected set; }

        [Tooltip("Set a keyword to invoke the OnSelect event")]
        public string Keyword = "";

        [Tooltip("Gaze is required for the keyword to trigger this Interactive.")]
        public bool KeywordRequiresGaze = true;

        /// <summary>
        /// Exposed Unity Events
        /// </summary>
        public UnityEvent OnFocusEvents;
        public UnityEvent OnUnfocusEvents;
        public UnityEvent OnSelectEvents;
        public UnityEvent OnDownEvent;
        public UnityEvent OnHoldEvent;

        /// <summary>
        /// A button typically has 8 potential states.
        /// We can update visual feedback based on state change, all the logic is already done, making InteractiveEffects behaviors less complex then comparing selected + Disabled.
        /// </summary>
        public enum ButtonStateEnum { Default, Focus, Press, Selected, FocusSelected, PressSelected, Disabled, DisabledSelected }

        public string CallerName
        {
            set { callerName = value; }
        }
        
        protected ButtonStateEnum mState = ButtonStateEnum.Default;

        /// <summary>
        /// Timers
        /// </summary>
        protected float mRollOffTimer = 0;
        protected float mHoldTimer = 0;
        protected bool mCheckRollOff = false;
        protected bool mCheckHold = false;

#if UNITY_WSA || UNITY_STANDALONE_WIN
        protected KeywordRecognizer mKeywordRecognizer;
#endif
        protected Dictionary<string, int> mKeywordDictionary;
        protected string[] mKeywordArray;

        /// <summary>
        /// Internal comparison variables to allow for live state updates no matter the input method
        /// </summary>
        protected bool mIgnoreSelect = false;
        protected bool mCheckEnabled = false;
        protected bool mCheckSelected = false;
        protected bool UserInitiatedEvent = false;
        protected bool mAllowSelection = false;

        protected List<InteractiveWidget> mInteractiveWidgets = new List<InteractiveWidget>();

        private string callerName;

        /// <summary>
        /// Set default visual states on Start
        /// </summary>
        protected virtual void Start()
        {
            if (Keyword != "")
            {
                mKeywordArray = new string[1] { Keyword };
                if (Keyword.IndexOf(',') > -1)
                {
                    mKeywordArray = Keyword.Split(',');

                    mKeywordDictionary = new Dictionary<string, int>();
                    for (int i = 0; i < mKeywordArray.Length; ++i)
                    {
                        mKeywordDictionary.Add(mKeywordArray[i], i);
                    }
                }

#if UNITY_WSA || UNITY_STANDALONE_WIN
                if (!KeywordRequiresGaze)
                {
                    mKeywordRecognizer = new KeywordRecognizer(mKeywordArray);
                    mKeywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
                    mKeywordRecognizer.Start();
                }
#endif

            }

            mCheckEnabled = IsEnabled;
            mCheckSelected = IsSelected;
            
        }

        /// <summary>
        /// An OnTap event occurred
        /// </summary>
        public virtual void OnInputClicked(InputClickedEventData eventData)
        {
            if (!IsEnabled)
            {
                return;
            }

            UserInitiatedEvent = true;

            if (mIgnoreSelect)
            {
                mIgnoreSelect = false;
                return;
            }

            OnSelectEvents.Invoke();
        }

        /// <summary>
        /// The gameObject received gaze
        /// </summary>
        public virtual void OnFocusEnter()
        {
            if (!IsEnabled)
            {
                return;
            }

            HasGaze = true;

            OnFocusEvents.Invoke();

        }

        /// <summary>
        /// The gameObject no longer has gaze
        /// </summary>
        public virtual void OnFocusExit()
        {
            HasGaze = false;
            EndHoldDetection();
            mRollOffTimer = 0;
            mCheckRollOff = true;
            
            OnUnfocusEvents.Invoke();
            
        }

        /// <summary>
        /// The user is initiating a tap or hold
        /// </summary>
        public virtual void OnInputDown(InputEventData eventData)
        {
            if (!HasGaze)
            {
                return;
            }

            HasDown = true;
            mCheckRollOff = false;

            if (DetectHold)
            {
                mHoldTimer = 0;
                mCheckHold = true;
            }
            
            OnDownEvent.Invoke();
            
            // Invoke method for showing our headplaceholder
            if(isForPositioning)
                DisplayHeadPlacement();
        }

        /// <summary>
        /// All tab, hold, and gesture events are completed
        /// </summary>
        public virtual void OnInputUp(InputEventData eventData)
        {

            mCheckHold = false;
            HasDown = false;
            mIgnoreSelect = false;
            EndHoldDetection();
            mCheckRollOff = false;

        }

        /// <summary>
        /// The hold timer has finished
        /// </summary>
        public virtual void OnHold()
        {
            mIgnoreSelect = true;
            EndHoldDetection();

            OnHoldEvent.Invoke();
        }

        /// <summary>
        /// The percentage of hold time completed
        /// </summary>
        /// <returns>percentage 0 - 1</returns>
        public float GetHoldPercentage()
        {
            return mHoldTimer / HoldTime;
        }

        protected void EndHoldDetection()
        {
            mHoldTimer = 0;
            mCheckHold = false;
        }

#if UNITY_WSA || UNITY_STANDALONE_WIN
        protected virtual void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {

            // Check to make sure the recognized keyword matches, then invoke the corresponding method.
            if (args.text == Keyword && (!KeywordRequiresGaze || HasGaze) && IsEnabled)
            {
                if (mKeywordDictionary == null)
                {
                    OnInputClicked(null);
                }
            }
        }
#endif
        /// <summary>
        /// Run timers and check for updates
        /// </summary>
        protected virtual void Update()
        {

            if (mCheckRollOff && HasDown)
            {
                if (mRollOffTimer < RollOffTime)
                {
                    mRollOffTimer += Time.deltaTime;
                }
                else
                {
                    mCheckRollOff = false;
                    OnInputUp(null);
                }
            }
            if (mCheckHold)
            {
                if (mHoldTimer < HoldTime)
                {
                    mHoldTimer += Time.deltaTime;
                }
                else
                {
                    mCheckHold = false;
                    OnHold();
                }
            }
            
            UserInitiatedEvent = false;
        }

        protected void OnDisable()
        {
            OnFocusExit();
        }

        private void DisplayHeadPlacement()
        {
          
            HeadAnchoring headAnchor = Instantiate(Resources.Load<GameObject>("HeadPlaceholder"), Vector3.zero, Quaternion.identity).GetComponent<HeadAnchoring>();
            headAnchor.EventName = callerName;
            
        }
    }
}
