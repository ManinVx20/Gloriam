using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class PlayerCanvas : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.forward = Vector3.down;
        }
    }
}
