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
        [SerializeField]
        private LayerMask _obstacleLayerMask;

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
                Vector3 spawnPosition = GetSuitableSpawnPosition();

                Bot bot = _botPool.GetPrefabInstance();
                bot.SetPosition(spawnPosition);
            }
        }

        private Vector3 GetSuitableSpawnPosition()
        {
            Vector3 spawnPosition = Vector3.zero;

            do
            {
                Vector2 direction = Random.insideUnitCircle.normalized;
                float distance = Random.Range(10.0f, 75.0f);
                spawnPosition = distance * new Vector3(direction.x, 0.0f, direction.y);
            } while (Physics.CheckSphere(spawnPosition, 10.0f, _obstacleLayerMask));

            return spawnPosition;
        }
    }
}
