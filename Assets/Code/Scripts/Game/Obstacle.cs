using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField]
        private Material _normalMaterial;
        [SerializeField]
        private Material _transparentMaterial;

        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            SetNormalMaterial();
        }

        public void SetNormalMaterial()
        {
            meshRenderer.material = _normalMaterial;
        }

        public void SetTrasparentMaterial()
        {
            meshRenderer.material = _transparentMaterial;
        }
    }
}
