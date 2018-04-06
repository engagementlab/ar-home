using HoloToolkit.Unity.SharingWithUNET;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    Text textMesh;
    public Transform AnchorPos;
    public NetworkManager NetManager;
    public UNetAnchorManager AnchorManager;



    

    // Use this for initialization
    void Start()
    {
        textMesh = gameObject.GetComponentInChildren<Text>();
        //Application.logMessageReceived += LogMessage;

    }

    private void Update()
    {
        textMesh.text = "Anchor Position: " + AnchorPos.position.ToString() + "\n";
        textMesh.text += "Number of Players: " + NetManager.numPlayers.ToString() + "\n";
        textMesh.text += "Net Address: " + NetManager.networkAddress.ToString() + "\n";
        textMesh.text += "Is Anchor Esatblished: " + AnchorManager.AnchorEstablished.ToString() + "\n";
    }

    public void LogMessage(string message, string stackTrace, LogType type)
    {
        if (textMesh.text.Length > 300)
        {
            textMesh.text = message + "\n";
        }
        else
        {
            textMesh.text += message + "\n";
        }
    }
}