using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private void Start()
        {
            ReturnToMainMenu();
        }

        public void StartGame()
        {
            ResourceManager.Instance.CreateMatch();

            UIManager.Instance.CloseAllUICanvases();
            UIManager.Instance.OpenUICanvas<GameplayCanvas>();
            UIManager.Instance.OpenUICanvas<ControlCanvas>();
            UIManager.Instance.OpenUICanvas<MatchCanvas>();
        }

        public void ReturnToMainMenu()
        {
            ResourceManager.Instance.RemoveMatch();

            Player.Instance.Spawn();

            UIManager.Instance.CloseAllUICanvases();
            UIManager.Instance.OpenUICanvas<MainMenuCanvas>();
        }
    }
}
