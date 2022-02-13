using System;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        private int team;

        public delegate void HitEntity(Entity entity);
        public event HitEntity OnHitEntity;

        public delegate void DestroyEntity(Entity entity);
        public event DestroyEntity OnDestroyEntity;
    
        public virtual void SetTeam(int team)
        {
            this.team = team;
        }

        public int GetTeam()
        {
            return team;
        }

        public void Destroy()
        {
            OnDestroyEntity?.Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var otherEntity = col.GetComponent<Entity>();
            if (otherEntity && GetTeam() != otherEntity.GetTeam() ||
                col.CompareTag("Ground"))
            {
                OnHitEntity?.Invoke(otherEntity);
            }
        }
    }
}

