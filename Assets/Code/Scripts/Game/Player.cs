using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        protected override void HandleInput(out Vector2 moveInput, out Vector2 lookInput)
        {
            moveInput = _playerInput.GetMoveInput();
            lookInput = _playerInput.GetLookInput();
        }
    }
}
