using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManager))]
public class Autohost : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<NetworkManager>().StartHost();
    }
}