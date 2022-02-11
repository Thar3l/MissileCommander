using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    public class ObjectPooler<T> where T : MonoBehaviour
    {
        private T prefab;
        Stack<T> objects;

        public delegate void CreateNewInstance(T obj);
        public event CreateNewInstance OnCreateNewInstance;
    
        public ObjectPooler(T prefab)
        {
            this.prefab = prefab;
            objects = new Stack<T>();
        }

        public T GetObject()
        {
            if (!objects.Any())
            {
                var obj = Object.Instantiate(prefab);
                OnCreateNewInstance?.Invoke(obj);
                return obj;
            }
            return objects.Pop();
        }

        public void AddObject(T obj)
        {
            obj.gameObject.SetActive(false);
            objects.Push(obj);
        }
    }
}

