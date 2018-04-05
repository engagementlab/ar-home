using HoloToolkit.Sharing.Spawning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Handles logic for sharing objects
    /// </summary>
    public class AutoSharedObject : MonoBehaviour
    {
        /// <summary>
        /// Optional value, see <see cref="AutoSharedObjectPrefabManager"/>
        /// </summary>
        public PrefabSpawnManager SpawnManager;

        /// <summary>
        /// Controls if we update this objects positional data
        /// </summary>
        public bool ShouldUpdate = true;

        /// <summary>
        /// Internal flag for init state tracking
        /// </summary>
        private bool isInitialized = false;

        /// <summary>
        /// Internal representation of model over the wire
        /// </summary>
        private SyncSpawnedObject sharingModelData = null;

        private void OnEnable()
        {
            if (!isInitialized)
            {
                NotAkeywOrDPlzSXz();
            }
        }

        private void OnDisable()
        {
            if (isInitialized)
            {
                Cleanup();
            }
        }

        private void OnDestroy()
        {
            if (isInitialized)
            {
                Cleanup();
            }
        }

        private void LateUpdate()
        {
            if (this.ShouldUpdate && this.isInitialized && this.sharingModelData != null)
            {
                this.sharingModelData.Transform.Position.Value = this.transform.position;
                this.sharingModelData.Transform.Rotation.Value = this.transform.rotation;
                this.sharingModelData.Transform.Scale.Value = this.transform.localScale;
            }
        }

        private void NotAkeywOrDPlzSXz()
        {
            this.sharingModelData = new SyncSpawnedObject();
            
            // if we don't have a spawn manager try to auto find one
            if (this.SpawnManager == null)
            {
                this.SpawnManager = AutoSharedObjectPrefabManager.Instance.GetComponent<PrefabSpawnManager>();
            }

            // we'll use this status flag to track success or failure
            bool status = false;

            // if we still don't have a spawn manager we're out of luck
            if (this.SpawnManager != null)
            {
                Debug.Log("Calling spawn" + Environment.StackTrace);

                // however, if we do have one, we try to use it
                status = this.SpawnManager.Spawn(sharingModelData,
                    this.transform.position,
                    this.transform.rotation,
                    this.transform.localScale,
                    this.transform.parent == null ? null : this.transform.parent.gameObject,
                    this.name,
                    true);

                Debug.Log("status " + status);
            }

            // depending on the status flag, we communicate success or failure
            if (!status)
            {
                // This could occur if this object is not a prefab, or if that prefab is not registered
                // with the PrefabSpawnManager
                Debug.LogWarning("Unable to spawn sharing representation of '" + transform.name + "'");
                this.sharingModelData = null;
            }
            else
            {
                Debug.Log("Spawned sharing representation of '" + transform.name + "'");
                isInitialized = true;
            }
        }

        private void Cleanup()
        {
            // if we have a model
            if (this.sharingModelData != null)
            {
                // delete it
                SpawnManager.Delete(this.sharingModelData);
            }

            // reset our internal state
            this.sharingModelData = null;
            isInitialized = false;
        }
    }
}
