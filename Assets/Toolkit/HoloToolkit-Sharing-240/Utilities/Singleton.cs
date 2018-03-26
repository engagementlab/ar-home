using UnityEngine;

namespace Academy.HoloToolkit.Unity
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _Instance;
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    Debug.Log("Find: " + typeof(T));
                    _Instance = FindObjectOfType<T>();
                }
                return _Instance;
            }
        }
    }
}