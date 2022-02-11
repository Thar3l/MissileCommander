using System;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        private int team;

        protected delegate void HitEntity(Entity entity);
        protected event HitEntity OnHitEntity;
    
        public virtual void SetTeam(int team)
        {
            this.team = team;
        }

        protected int GetTeam()
        {
            return team;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var entity = col.GetComponent<Entity>();
            if (entity && GetTeam() != entity.GetTeam() ||
                col.CompareTag("Ground"))
            {
                OnHitEntity?.Invoke(entity);
            }
        }
    }
}

