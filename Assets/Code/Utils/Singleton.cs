using UnityEngine;

namespace GameUtils
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindInstance();
                return _instance;
            }
        }

        static T FindInstance()
        {
            var instances = FindObjectsOfType<T>();
            if (instances.Length > 0)
                return instances[0];
            return new GameObject($"{typeof(T)}").AddComponent<T>();
        }
    }
}
