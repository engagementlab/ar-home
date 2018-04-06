using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts
{
    public class NetCubeData : NetworkBehaviour
    {
        [SyncVar(hook = "OnTimeSinceSpawnChanged")]
        private float timeSinceSpawn = 0;

        private void OnTimeSinceSpawnChanged(float newTimeValue)
        {
            if (isClient && !hasAuthority)
            {
                Debug.Log("Got NetCubeData.timeSinceSpawn update: " + newTimeValue + " (" + timeSinceSpawn + ")");
            }
        }

        private void Update()
        {
            if (hasAuthority)
            {
                timeSinceSpawn += Time.deltaTime;
            }
        }
    }
}
