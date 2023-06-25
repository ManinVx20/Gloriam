using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class Bot : Character
    {
        private Vector2 _botMoveInput;
        private Vector2 _botLookInput;
        private float _changeLookInputTimer;
        private float _changeLookInputTimerMax;
        private IBotState _botState;

        public override void Spawn()
        {
            base.Spawn();

            _changeLookInputTimer = 0.0f;
            _changeLookInputTimerMax = Random.Range(0.5f, 1.0f);

            ChangeBotState(new IdleBotState());
        }

        public override void Despawn()
        {
            base.Despawn();

            ReturnToPool();
        }

        public override void Execute()
        {
            base.Execute();

            HandleLook();

            _botState?.Execute(this);
        }

        public void SetMoveInput(Vector2 moveInput)
        {
            _botMoveInput = moveInput;
        }

        public void SetLookInput(Vector2 lookInput)
        {
            _botLookInput = lookInput;
        }

        public void ChangeBotState(IBotState botState)
        {
            this._botState?.Exit(this);

            this._botState = botState;

            this._botState?.Enter(this);
        }

        protected override void HandleDeath(Character character)
        {
            base.HandleDeath(character);

            ChangeBotState(null);
        }

        protected override void HandleInput(out Vector2 moveInput, out Vector2 lookInput)
        {
            moveInput = _botMoveInput;
            lookInput = _botLookInput;
        }

        private void HandleLook()
        {
            if (!IsAttacking())
            {
                _changeLookInputTimer += Time.deltaTime;
                if (_changeLookInputTimer >= _changeLookInputTimerMax)
                {
                    _changeLookInputTimer = 0.0f;
                    _changeLookInputTimerMax = Random.Range(0.5f, 1.0f);

                    SetLookInput(Utilities.GetRandomNormalizedVector2(-1.0f, 1.0f));
                }
            }
            else if (TargetCharacterInRange() != null)
            {
                Vector3 targetPosition = TargetCharacterInRange().transform.position;
                Vector3 lookDirection = (targetPosition - transform.position).normalized;

                SetLookInput(new Vector2(lookDirection.x, lookDirection.z).normalized);
            }
        }
    }
}
