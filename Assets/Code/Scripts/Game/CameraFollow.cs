using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetTransform;
        [SerializeField]
        private float _offsetX, _offsetZ;
        [SerializeField]
        private float _lerpSpeed;

        private void LateUpdate()
        {
            if ( _targetTransform != null )
            {
                Vector3 targetPosition = new Vector3(_targetTransform.position.x + _offsetX, transform.position.y, _targetTransform.position.z + _offsetZ);
                transform.position = Vector3.Lerp(transform.position, targetPosition, _lerpSpeed * Time.deltaTime);
            }
        }
    }
}
