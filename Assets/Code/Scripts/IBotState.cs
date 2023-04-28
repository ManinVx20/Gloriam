using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public interface IBotState
    {
        public void Enter(Bot bot);

        public void Execute(Bot bot);

        public void Exit(Bot bot);
    }
}
