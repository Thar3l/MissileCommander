using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour
    {
        private int _team;

        public delegate void EntityAction(Entity entity);
        public event EntityAction OnHitEntity;
        public event EntityAction OnDestroyEntity;
    
        public virtual void SetTeam(int team)
        {
            _team = team;
        }

        public int GetTeam()
        {
            return _team;
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

