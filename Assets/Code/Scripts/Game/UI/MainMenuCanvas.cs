using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StormDreams
{
    public class MainMenuCanvas : UICanvas
    {
        [SerializeField]
        private Button _startGameButton;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(() =>
            {
                GameManager.Instance.StartGame();
            });
        }
    }
}
