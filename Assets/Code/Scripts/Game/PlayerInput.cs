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
            return _moveJoystick.Input;
            //return _gameControls.Player.Move.ReadValue<Vector2>();
        }

        public Vector2 GetLookInput()
        {
            return _lookJoystick.Input;
            //return gameControls.Player.Look.ReadValue<Vector2>();
        }
    }
}
