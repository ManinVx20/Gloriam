using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class PoolableObject : MonoBehaviour, IPoolable
    {
        public IObjectPool Pool { get; set; }

        public virtual void PrepareToUse()
        {

        }

        public virtual void ReturnToPool()
        {
            if (Pool != null && Pool.Exist())
            {
                Pool.ReturnToPool(this);
            }
            else
            {
                Dispose();
            }
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }
    }
}
