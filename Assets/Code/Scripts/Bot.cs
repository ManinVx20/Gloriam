using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class Bot : Character
    {
        private Vector2 botMoveInput;
        private Vector2 botLookInput;
        private IBotState botState;

        public override void Spawn()
        {
            base.Spawn();

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

            botState?.Execute(this);
        }

        public void SetMoveInput(Vector2 moveInput)
        {
            botMoveInput = moveInput;
        }

        public void SetLookInput(Vector2 lookInput)
        {
            botLookInput = lookInput;
        }

        public void ChangeBotState(IBotState botState)
        {
            if (botState == this.botState)
            {
                return;
            }

            this.botState?.Exit(this);

            this.botState = botState;

            this.botState?.Enter(this);
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();

            ChangeBotState(null);
        }

        protected override void HandleInput(out Vector2 moveInput, out Vector2 lookInput)
        {
            moveInput = botMoveInput;
            lookInput = botLookInput;
        }
    }
}
