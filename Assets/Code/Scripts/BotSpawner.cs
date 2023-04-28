using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class BotSpawner : MonoBehaviour
    {
        private BotPool _botPool;

        private void Awake()
        {
            _botPool = GetComponent<BotPool>();
        }

        private void Start()
        {
            Bot bot = _botPool.GetPrefabInstance();
            bot.SetPosition(new Vector3(6.0f, 0.0f, 3.0f)); 
        }
    }
}
