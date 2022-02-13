using System.Collections.Generic;
using UnityEngine;
using GameUtils;

namespace Entities.Factories
{
    public abstract class EntityFactory<T> where T : Entity
    {
        protected readonly ObjectPooler<T> EntityPooler;
        
        #region events
        public delegate void CreateEntityEvent(T entity);
        public event CreateEntityEvent OnCreateEntityEvent;
        public delegate void DestroyEntityEvent(T entity);
        public event DestroyEntityEvent OnDestroyEntityEvent;
        #endregion

        protected EntityFactory(T prefab)
        {
            EntityPooler = new ObjectPooler<T>(prefab);
            EntityPooler.OnCreateNewInstance += SetNewEntityProperties;
        }

        protected virtual void SetNewEntityProperties(T obj)
        {
            obj.OnDestroyEntity += (x) => DestroyEntity(obj);
        }
        

        protected void DestroyEntity(T entity)
        {
            EntityPooler.AddObject(entity);
            OnDestroyEntityEvent?.Invoke(entity);
        }

        protected T CreateEntity(Vector2 spawnPosition)
        {
            var entity = EntityPooler.GetObject();
            entity.transform.position = spawnPosition;
            OnCreateEntityEvent?.Invoke(entity);
            return entity;
        }
    }
}
