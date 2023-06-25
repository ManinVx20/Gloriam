using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class ColliderTrigger : MonoBehaviour
    {
        public class OnCharacterTriggeredEventArgs : EventArgs
        {
            public Character Character;
        }

        public event EventHandler<OnCharacterTriggeredEventArgs> OnCharacterEntered;
        public event EventHandler<OnCharacterTriggeredEventArgs> OnCharacterExited;

        private void OnTriggerEnter(Collider collider)
        {
            OnCharacterEntered?.Invoke(this, new OnCharacterTriggeredEventArgs
            {
                Character = collider.GetComponent<Character>()
            });
        }

        private void OnTriggerExit(Collider collider)
        {
            OnCharacterExited?.Invoke(this, new OnCharacterTriggeredEventArgs
            {
                Character = collider.GetComponent<Character>()
            });
        }
    }
}
