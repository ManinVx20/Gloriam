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
        private float _matchTimer;
        private int _aliveCount;
        private int _killsCount;
        private bool _ended;

        private void Awake()
        {
            NavMesh.RemoveAllNavMeshData();

            _navMeshDataInstance = NavMesh.AddNavMeshData(_navMeshData);
        }

        private void Start()
        {
            StartMatch();
        }

        private void OnDestroy()
        {
            NavMesh.RemoveNavMeshData(_navMeshDataInstance);

            Character.OnAnyCharacterDead -= Character_OnAnyCharacterDead;
        }

        private void Update()
        {
            _matchTimer += Time.deltaTime;

            UIManager.Instance.GetUICanvas<MatchCanvas>().SetMatchTimerText(_matchTimer);
        }

        public void StartMatch()
        {
            Character.OnAnyCharacterDead += Character_OnAnyCharacterDead;

            _aliveCount = 50;
            _killsCount = 0;

            UIManager.Instance.GetUICanvas<MatchCanvas>().SetAliveText(_aliveCount);
            UIManager.Instance.GetUICanvas<MatchCanvas>().SetKillsText(_killsCount);
        }

        public void EndMatch(bool won)
        {
            _ended = true;

            UIManager.Instance.GetUICanvas<ControlCanvas>().Close();

            if (won)
            {
                UIManager.Instance.GetUICanvas<MatchCanvas>().OpenWinUI(_killsCount);
            }
            else
            {
                UIManager.Instance.GetUICanvas<MatchCanvas>().OpenLoseUI(_killsCount);
            }
        }

        public void Despawn()
        {
            _botSpawner.Despawn();

            Destroy(gameObject);
        }

        public float GetMatchTimer()
        {
            return _matchTimer;
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

            if (!_ended)
            {
                if (sender is Player)
                {
                    EndMatch(false);
                }
                else if (_aliveCount == 1)
                {
                    EndMatch(true);
                }
            }
        }
    }
}
