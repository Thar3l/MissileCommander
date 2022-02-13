using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player
{
    public class City : Entity
    {
        void Awake()
        {
            OnHitEntity += (entity) => gameObject.SetActive(false);
        }
    }
}
