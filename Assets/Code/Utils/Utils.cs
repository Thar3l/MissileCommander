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
    }
}

