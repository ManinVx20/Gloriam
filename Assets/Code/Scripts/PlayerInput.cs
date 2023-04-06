using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StormDreams
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private Joystick moveJoystick;
        [SerializeField]
        private Joystick lookJoystick;

        private GameControls gameControls;

        private void Awake()
        {
            gameControls = new GameControls();

            gameControls.Enable();
        }

        private void OnDestroy()
        {
            gameControls.Disable();
        }

        public Vector2 GetMoveInput()
        {
            return gameControls.Player.Move.ReadValue<Vector2>();
        }

        public Vector2 GetLookInput()
        {
            //return gameControls.Player.Look.ReadValue<Vector2>();
            return lookJoystick.Input;
        }
    }
}
