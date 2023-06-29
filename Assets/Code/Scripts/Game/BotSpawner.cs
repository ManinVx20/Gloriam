using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class BotSpawner : MonoBehaviour
    {
        [SerializeField]
        private BotPool _botPool;
        [SerializeField]
        private int _maxBotCount = 49;

        private void Start()
        {
            SpawnBots();
        }

        public void Despawn()
        {
            _botPool.Dispose();

            Destroy(gameObject);
        }

        private void SpawnBots()
        {
            for (int i = 0; i < _maxBotCount; i++)
            {
                Bot bot = _botPool.GetPrefabInstance();
            }
        }
    }
}
