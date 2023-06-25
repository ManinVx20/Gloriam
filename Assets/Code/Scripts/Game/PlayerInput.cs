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
        private Joystick _moveJoystick;
        [SerializeField]
        private Joystick _lookJoystick;

        private GameControls _gameControls;

        private void Awake()
        {
            _gameControls = new GameControls();

            _gameControls.Enable();
        }

        private void OnDestroy()
        {
            _gameControls.Disable();
        }

        public Vector2 GetMoveInput()
        {
            return _gameControls.Player.Move.ReadValue<Vector2>();
            //return _moveJoystick.Input;
        }

        public Vector2 GetLookInput()
        {
            //return gameControls.Player.Look.ReadValue<Vector2>();
            return _lookJoystick.Input;
        }
    }
}
