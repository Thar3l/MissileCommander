using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    public class ObjectPooler<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        readonly Stack<T> _objects;

        public delegate void CreateNewInstance(T obj);
        public event CreateNewInstance OnCreateNewInstance;
    
        public ObjectPooler(T prefab)
        {
            this._prefab = prefab;
            _objects = new Stack<T>();
        }

        public T GetObject()
        {
            if (!_objects.Any())
            {
                var obj = Object.Instantiate(_prefab);
                OnCreateNewInstance?.Invoke(obj);
                return obj;
            }
            return _objects.Pop();
        }

        public void AddObject(T obj)
        {
            obj.gameObject.SetActive(false);
            _objects.Push(obj);
        }
    }
}

