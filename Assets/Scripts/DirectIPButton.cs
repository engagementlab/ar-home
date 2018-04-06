// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.Events;

namespace HoloToolkit.Unity.SharingWithUNET
{

    public class DirectIPButton : MonoBehaviour, IInputClickHandler
    {
        public UnityEvent OnClick;
        
        /// <summary>
        /// When the button is clicked try to join the selected session
        /// </summary>
        /// <param name="eventData"></param>
        public void OnInputClicked(InputClickedEventData eventData)
        {
            if (this.OnClick != null)
            {
                this.OnClick.Invoke();
            }

            eventData.Use();
        }
    }
}
