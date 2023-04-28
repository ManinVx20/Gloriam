using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public interface IPoolable
    {
        IObjectPool Pool { get; set; }

        void PrepareToUse();
        void ReturnToPool();
    }
}
