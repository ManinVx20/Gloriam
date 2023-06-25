using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class LineRendererCircle : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void DrawCircle(float radius, Vector3 offset)
        {
            _lineRenderer.positionCount = 0;

            int segments = 360;
            int positionCount = segments + 1;

            Vector3[] positionArray = new Vector3[positionCount];
            for (int i = 0; i < positionCount; i++)
            {
                float rad = Mathf.Deg2Rad * (i * 360.0f / segments);
                positionArray[i] = new Vector3(Mathf.Sin(rad) * radius, 0.0f, Mathf.Cos(rad) * radius) + offset;
            }

            _lineRenderer.positionCount = positionCount;
            _lineRenderer.SetPositions(positionArray);
        }

        public void Clear()
        {
            _lineRenderer.positionCount = 0;
        }
    }
}
