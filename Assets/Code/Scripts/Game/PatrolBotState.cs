using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class PatrolBotState : IBotState
    {
        private float _changeStateTimer;
        private float _changeStateTimerMax;

        public void Enter(Bot bot)
        {
            bot.SetMoveInput(Utilities.GetRandomNormalizedVector2(-1.0f, 1.0f));

            _changeStateTimer = 0.0f;
            _changeStateTimerMax = Random.Range(1.0f, 3.0f);
        }

        public void Execute(Bot bot)
        {
            _changeStateTimer += Time.deltaTime;
            if (_changeStateTimer >= _changeStateTimerMax)
            {
                if (Random.Range(0, 3) > 1)
                {
                    bot.ChangeBotState(new IdleBotState());
                }
                else
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
