using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Test_SpawnAfterConnectionSuccess : MonoBehaviour
    {
        public float DelaySeconds = 5f;
        public GameObject Prefab;

        private void Start()
        {
            SharingStage.Instance.SessionsTracker.CurrentUserJoined += SessionsTracker_CurrentUserJoined;
        }

        private void SessionsTracker_CurrentUserJoined(Session obj)
        {
            Debug.Log("Joined " + obj.GetName().ToString());

            StartCoroutine(SpawnObject());
        }

        private IEnumerator SpawnObject()
        {
            yield return new WaitForSeconds(this.DelaySeconds);

            var mgr = AutoSharedObjectPrefabManager.Instance.GetComponent<PrefabSpawnManager>();

            Debug.Log("Calling spawn" + Environment.StackTrace);

            // however, if we do have one, we try to use it
            var status = mgr.Spawn(new SyncSpawnedObject(),
                new Vector3(0,1,1),
                Prefab.transform.rotation,
                this.transform.parent == null ? null : this.transform.parent.gameObject,
                this.name,
                true);

            Debug.Log("status " + status);
        }
    }
}
