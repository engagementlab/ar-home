using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class UnityEventDebugger : MonoBehaviour
    {
        public void Log(string data)
        {
            Debug.Log(data);
        }
    }
}
