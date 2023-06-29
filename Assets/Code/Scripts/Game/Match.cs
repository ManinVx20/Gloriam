using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace StormDreams
{
    public class Match : MonoBehaviour
    {
        [SerializeField]
        private NavMeshData _navMeshData;
        [SerializeField]
        private BotSpawner _botSpawner;

        private NavMeshDataInstance _navMeshDataInstance;
        private int _aliveCount;
        private int _killsCount;

        private void Awake()
        {
            NavMesh.RemoveAllNavMeshData();

            _navMeshDataInstance = NavMesh.AddNavMeshData(_navMeshData);
        }

        private void Start()
        {
            Character.OnAnyCharacterDead += Character_OnAnyCharacterDead;

            _aliveCount = 50;
            _killsCount = 0;

            UIManager.Instance.GetUICanvas<MatchCanvas>().SetAliveText(_aliveCount);
            UIManager.Instance.GetUICanvas<MatchCanvas>().SetKillsText(_killsCount);
        }

        private void OnDestroy()
        {
            Character.OnAnyCharacterDead -= Character_OnAnyCharacterDead;

            NavMesh.RemoveNavMeshData(_navMeshDataInstance);
        }

        public void Despawn()
        {
            _botSpawner.Despawn();

            Destroy(gameObject);
        }

        public int GetAliveCount()
        {
            return _aliveCount;
        }

        public int GetKillsCount()
        {
            return _killsCount;
        }

        public void ReduceAliveCount()
        {
            _aliveCount -= 1;

            UIManager.Instance.GetUICanvas<MatchCanvas>().SetAliveText(_aliveCount);
        }

        public void IncreaseKillsCount()
        {
            _killsCount += 1;

            UIManager.Instance.GetUICanvas<MatchCanvas>().SetKillsText(_killsCount);
        }

        private void Character_OnAnyCharacterDead(object sender, Character.OnCharacterDeadArgs args)
        {
            ReduceAliveCount();

            if (args.Character != null)
            {
                if (!args.Character.IsDead())
                {
                    args.Character.GainKill();
                }

                if (args.Character is Player)
                {
                    IncreaseKillsCount();
                }
            }

            if (!GameManager.Instance.IsGameOver())
            {
                if (sender is Player)
                {
                    GameManager.Instance.EndGame(false);
                }
                else if (_aliveCount == 1)
                {
                    GameManager.Instance.EndGame(true);
                }
            }
        }
    }
}
