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
        private PlayerInput playerInput;

        protected override void Awake()
        {
            base.Awake();

            playerInput = GetComponent<PlayerInput>();
        }

        protected override void HandleInput(out Vector2 moveInput, out Vector2 lookInput)
        {
            moveInput = playerInput.GetMoveInput();
            lookInput = playerInput.GetLookInput();
        }
    }
}
