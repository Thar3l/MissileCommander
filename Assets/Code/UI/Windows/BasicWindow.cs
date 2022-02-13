using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows
{
    public abstract class BasicWindow : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}

