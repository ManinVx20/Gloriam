using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class GameManager : MonoSingleton<GameManager>
    {
        [Serializable]
        public enum GameState
        {
            WaitingToStart = 0,
            CountdownToStart = 1,
            GamePlaying = 2,
            GameOver = 3,
        }

        public event EventHandler OnGameStateChanged;

        private GameState _gameState;
        private Match _match;
        private float _waitingToStartTimer = 0.0f;
        private float _countdownToStartTimer = 3.0f;
        private float _gamePlayingTimer = 0.0f;

        private void Start()
        {
            ReturnToMainMenu();
        }

        private void Update()
        {
            switch (_gameState)
            {
                case GameState.WaitingToStart:
                    //_waitingToStartTimer -= Time.deltaTime;
                    //if (_waitingToStartTimer <= 0.0f)
                    //{
                    //    _gameState = GameState.CountdownToStart;
                    //    OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                    //}
                    break;
                case GameState.CountdownToStart:
                    _countdownToStartTimer -= Time.deltaTime;
                    UIManager.Instance.GetUICanvas<MatchCanvas>().SetCountdownToStartTimerText(_countdownToStartTimer);
                    if (_countdownToStartTimer <= 0.0f)
                    {
                        _gameState = GameState.GamePlaying;
                        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                    }
                    break;
                case GameState.GamePlaying:
                    _gamePlayingTimer += Time.deltaTime;
                    UIManager.Instance.GetUICanvas<MatchCanvas>().SetMatchTimerText(_gamePlayingTimer);
                    break;
                case GameState.GameOver:
                    break;
            }
        }

        public bool IsCountdownToStartActive()
        {
            return _gameState == GameState.CountdownToStart;
        }

        public bool IsGamePlaying()
        {
            return _gameState == GameState.GamePlaying;
        }

        public bool IsGameOver()
        {
            return _gameState == GameState.GameOver;
        }

        public float GetCountdownToStartTimer()
        {
            return _countdownToStartTimer;
        }

        public float GetGamePlayingTimer()
        {
            return _gamePlayingTimer;
        }

        public void StartGame()
        {
            _gameState = GameState.CountdownToStart;
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);

            _countdownToStartTimer = 3.0f;
            _gamePlayingTimer = 0.0f;

            if (_match == null)
            {
                _match = Instantiate(ResourceManager.Instance.MatchPrefab);
            }

            Player.Instance.Spawn();

            UIManager.Instance.CloseAllUICanvases();
            UIManager.Instance.OpenUICanvas<GameplayCanvas>();
            UIManager.Instance.OpenUICanvas<ControlCanvas>();
            UIManager.Instance.OpenUICanvas<MatchCanvas>();
        }

        public void EndGame(bool won)
        {
            _gameState = GameState.GameOver;
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);

            UIManager.Instance.GetUICanvas<ControlCanvas>().Close();

            if (won)
            {
                UIManager.Instance.GetUICanvas<MatchCanvas>().OpenWinUI(_match.GetKillsCount());
            }
            else
            {
                UIManager.Instance.GetUICanvas<MatchCanvas>().OpenLoseUI(_match.GetKillsCount());
            }
        }

        public void ReturnToMainMenu()
        {
            _gameState = GameState.WaitingToStart;
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);

            if (_match != null)
            {
                _match.Despawn();
            }

            UIManager.Instance.CloseAllUICanvases();
            UIManager.Instance.OpenUICanvas<MainMenuCanvas>();
        }
    }
}
