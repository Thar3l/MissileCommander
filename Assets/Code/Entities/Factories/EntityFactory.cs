using UnityEngine;
using GameUtils;

namespace Entities.Factories
{
    public abstract class EntityFactory<T> where T : MonoBehaviour
    {
        protected readonly ObjectPooler<T> EntityPooler;

        protected EntityFactory(T prefab)
        {
            EntityPooler = new ObjectPooler<T>(prefab);
            EntityPooler.OnCreateNewInstance += SetNewEntityProperties;
        }
        protected abstract void SetNewEntityProperties(T obj);

        protected void DestroyEntity(T entity)
        {
            EntityPooler.AddObject(entity);
        }

        protected T CreateEntity(Vector2 spawnPosition)
        {
            var entity = EntityPooler.GetObject();
            entity.transform.position = spawnPosition;
            return entity;
        }
    }
}
