using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class IdleBotState : IBotState
    {
        private float timer;
        private float timerMax;

        public void Enter(Bot bot)
        {
            bot.SetMoveInput(Vector2.zero);

            timer = 0.0f;
            timerMax = 1.0f;
        }

        public void Execute(Bot bot)
        {
            if (bot.IsAttacking())
            {
                bot.ChangeBotState(new AttackBotState());
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= timerMax)
                {
                    bot.ChangeBotState(new PatrolBotState());
                }
            }
        }

        public void Exit(Bot bot)
        {

        }
    }
}
