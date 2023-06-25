using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class IdleBotState : IBotState
    {
        private float _changeStateTimer;
        private float _changeStateTimerMax;

        public void Enter(Bot bot)
        {
            bot.SetMoveInput(Vector2.zero);

            _changeStateTimer = 0.0f;
            _changeStateTimerMax = 1.0f;
        }

        public void Execute(Bot bot)
        {
            _changeStateTimer += Time.deltaTime;
            if (_changeStateTimer >= _changeStateTimerMax)
            {
                if (Random.Range(0, 3) > 1)
                {
                    bot.ChangeBotState(new PatrolBotState());
                }
                else
                {
                    bot.ChangeBotState(new IdleBotState());
                }
            }
        }

        public void Exit(Bot bot)
        {

        }
    }
}
