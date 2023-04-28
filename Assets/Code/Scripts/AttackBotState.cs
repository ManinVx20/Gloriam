using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class AttackBotState : IBotState
    {
        public void Enter(Bot bot)
        {

        }

        public void Execute(Bot bot)
        {
            if (bot.IsAttacking())
            {
                bot.SetLookInput(GetLookInput(bot));
            }
            else
            {
                bot.ChangeBotState(new IdleBotState());
            }
        }

        public void Exit(Bot bot)
        {

        }

        private Vector2 GetLookInput(Bot bot)
        {
            Vector3 targetPosition = bot.FirstTargetCharacterInRange().transform.position;
            targetPosition.y = bot.transform.position.y;
            Vector3 lookDirection = (targetPosition - bot.transform.position).normalized;

            return new Vector2(lookDirection.x, lookDirection.z).normalized;
        }
    }
}
