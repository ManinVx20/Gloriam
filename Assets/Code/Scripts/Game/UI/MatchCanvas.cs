using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StormDreams
{
    public class MatchCanvas : UICanvas
    {
        [SerializeField]
        private TextMeshProUGUI _matchTimerText;
        [SerializeField]
        private TextMeshProUGUI _aliveText;
        [SerializeField]
        private TextMeshProUGUI _killsText;
        [SerializeField]
        private Image _shrinkingProgressImage;
        [SerializeField]
        private GameObject _circleCreatingNotificationUI;
        [SerializeField]
        private GameObject _circleShrinkingNotificationUI;
        [SerializeField]
        private GameObject _winUI;
        [SerializeField]
        private TextMeshProUGUI _winUIKillsText;
        [SerializeField]
        private Button _winUIExitButton;
        [SerializeField]
        private GameObject _loseUI;
        [SerializeField]
        private TextMeshProUGUI _loseUIKillsText;
        [SerializeField]
        private Button _loseUIExitButton;
        [SerializeField]
        private Button _pauseButton;
        [SerializeField]
        private GameObject _gameStartCountdownUI;
        [SerializeField]
        private TextMeshProUGUI _countdownText;

        private void Awake()
        {
            _winUIExitButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReturnToMainMenu();
            });
            _loseUIExitButton.onClick.AddListener(() =>
            {
                GameManager.Instance.ReturnToMainMenu();
            });
            _pauseButton.onClick.AddListener(() =>
            {
                UIManager.Instance.OpenUICanvas<PauseCanvas>();
            });

            GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        }

        public override void Setup()
        {
            base.Setup();

            if (!GameManager.Instance.IsGamePlaying())
            {
                _circleCreatingNotificationUI.SetActive(false);
                _circleShrinkingNotificationUI.SetActive(false);
                _winUI.SetActive(false);
                _loseUI.SetActive(false);
            }
        }

        public void SetMatchTimerText(float value)
        {
            int minutes = Mathf.FloorToInt(value / 60);
            int seconds = Mathf.FloorToInt(value % 60);

            _matchTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void SetCountdownToStartTimerText(float value)
        {
            _countdownText.text = Mathf.Ceil(value).ToString();
        }

        public void SetAliveText(int value)
        {
            _aliveText.text = value.ToString();
        }

        public void SetKillsText(int value)
        {
            _killsText.text = value.ToString();
        }

        public void SetShrinkingProgressFillValue(float percentage)
        {
            _shrinkingProgressImage.fillAmount = percentage;
        }

        public void OpenCircleCreatingNotificationUI()
        {
            _circleCreatingNotificationUI.SetActive(true);

            StartCoroutine(Utilities.DelayActionCoroutine(3.0f, () =>
            {
                _circleCreatingNotificationUI.SetActive(false);
            }));
        }

        public void OpenCircleShrinkingNotificationUI()
        {
            _circleShrinkingNotificationUI.SetActive(true);

            StartCoroutine(Utilities.DelayActionCoroutine(3.0f, () =>
            {
                _circleShrinkingNotificationUI.SetActive(false);
            }));
        }

        public void OpenWinUI(int killCount)
        {
            StartCoroutine(Utilities.DelayActionCoroutine(3.0f, () =>
            {
                _winUI.SetActive(true);
                _winUIKillsText.text = $"Kills: {killCount}";
            }));
        }

        public void OpenLoseUI(int killCount)
        {
            StartCoroutine(Utilities.DelayActionCoroutine(3.0f, () =>
            {
                _loseUI.SetActive(true);
                _loseUIKillsText.text = $"Kills: {killCount}";
            }));
        }

        private void GameManager_OnGameStateChanged(object sender, System.EventArgs args)
        {
            if (GameManager.Instance.IsCountdownToStartActive())
            {
                _gameStartCountdownUI.SetActive(true);
            }
            else
            {
                _gameStartCountdownUI.SetActive(false);
            }
        }
    }
}
