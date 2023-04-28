using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class UICanvas : MonoBehaviour
    {
        public virtual void Setup()
        {

        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void CloseWithDelay(float delayTime)
        {
            Invoke(nameof(Close), delayTime);
        }
    }
}
