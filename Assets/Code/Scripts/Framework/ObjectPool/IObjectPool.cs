using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public interface IObjectPool
    {
        void ReturnToPool(object instance);
    }

    public interface IObjectPool<T> : IObjectPool where T : IPoolable
    {
        T GetPrefabInstance();

        void ReturnToPool(T instance);

        void Dispose();
    }
}
