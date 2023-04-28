using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class PatrolBotState : IBotState
    {
        private float changeStateTimer;
        private float changeStateTimerMax;
        private float changeLookInputTimer;
        private float changeLookInputTimerMax;

        public void Enter(Bot bot)
        {
            bot.SetMoveInput(Utilities.GetRandomNormalizedVector2(-1.0f, 1.0f));

            changeStateTimer = 0.0f;
            changeStateTimerMax = 1.0f;

            changeLookInputTimer = 0.0f;
            changeLookInputTimerMax = 0.25f;
        }

        public void Execute(Bot bot)
        {
            if (bot.IsAttacking())
            {
                bot.ChangeBotState(new AttackBotState());
            }
            else
            {
                changeStateTimer += Time.deltaTime;
                if (changeStateTimer >= changeStateTimerMax)
                {
                    bot.ChangeBotState(new IdleBotState());
                }

                changeLookInputTimer += Time.deltaTime;
                if (changeLookInputTimer >= changeLookInputTimerMax)
                {
                    changeLookInputTimer = 0.0f;

                    bot.SetLookInput(Utilities.GetRandomNormalizedVector2(-1.0f, 1.0f));
                }
            }
        }

        public void Exit(Bot bot)
        {

        }
    }
}
