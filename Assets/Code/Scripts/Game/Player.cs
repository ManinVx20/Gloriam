using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace StormDreams
{
    public class Player : Character
    {
        public static Player Instance { get; private set; }

        [SerializeField]
        private GameObject _rangeIndicatorGameObject;
        [SerializeField]
        private GameObject _minimapIconGameObject;

        private PlayerInput _playerInput;

        public override void Initialize()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            base.Initialize();

            _playerInput = GetComponent<PlayerInput>();
        }

        public override void Spawn()
        {
            base.Spawn();

            SetPosition(Vector3.zero);
            transform.rotation = Quaternion.identity;

        }

        protected override void HandleInput(out Vector2 moveInput, out Vector2 lookInput)
        {
            moveInput = _playerInput.GetMoveInput();
            lookInput = _playerInput.GetLookInput();
        }
    }
}
