using HoloToolkit.Unity.SharingWithUNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    [RequireComponent(typeof(NetworkDiscoveryWithAnchors))]
    [RequireComponent(typeof(NetworkManager))]
    public class NetworkManagerDirectIP : MonoBehaviour
    {
        public void ConnectAsClient(string server)
        {
            this.GetComponent<NetworkDiscoveryWithAnchors>().StopListening();

            NetworkManager.singleton.networkAddress = server;
#if !UNITY_EDITOR && UNITY_WSA
            // Tell the network transmitter the IP to request anchor data from if needed.
            GenericNetworkTransmitter.Instance.SetServerIp(server);
#else
            Debug.LogWarning("This script will need modification to work in the Unity Editor");
#endif
            NetworkManager.singleton.StartClient();
        }
    }
}
