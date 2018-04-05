using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Tagging class to simplify AutoSharedObject workloads
    /// </summary>
    /// <remarks>
    /// To use, simply attach this component to some manager object
    /// and then use <see cref="AutoSharedObject"/> without the need to set
    /// <see cref="AutoSharedObject.SpawnManager"/>
    /// </remarks>
    [RequireComponent(typeof(PrefabSpawnManager))]
    class AutoSharedObjectPrefabManager : SingleInstance<AutoSharedObjectPrefabManager>
    {
    }
}
