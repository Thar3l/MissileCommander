using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public static class Utils
    {
        public static Vector2 MousePositionToWorldPoint(Camera cam, Vector2 mousePosition)
        {
            return cam.ScreenToWorldPoint(mousePosition);
        }
        
        public static Vector2 PickRandomPositionFromRange(Vector2 center, Vector2 range)
        {
            return new Vector2(
                center.x + Random.Range(-range.x/2, range.x/2),
                center.y + Random.Range(-range.y/2, range.y/2)
            );
        }
    }
}

