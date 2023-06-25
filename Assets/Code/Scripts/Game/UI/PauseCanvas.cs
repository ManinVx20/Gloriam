using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StormDreams
{
    public class PauseCanvas : UICanvas
    {
        [SerializeField]
        private Button _resumeButton;
        [SerializeField]
        private Button _exitButton;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(() =>
            {
                Close();
            });
            _exitButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReturnToMainMenu();
            });
        }
    }
}
