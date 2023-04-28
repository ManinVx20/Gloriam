using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StormDreams
{
    public class CharacterInfoUI : PoolableObject
    {
        [SerializeField]
        private Image healthBarFillImage;
        [SerializeField]
        private TextMeshProUGUI nameText;

        private RectTransform rectTransform;
        private Character character;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Initialize(Character character)
        {
            this.character = character;

            CinemachineCore.CameraUpdatedEvent.AddListener(UpdatePosition);

            character.OnCharacterHealthChanged += Character_OnCharacterHealthChanged;
        }

        public void Despawn()
        {
            character.OnCharacterHealthChanged -= Character_OnCharacterHealthChanged;

            CinemachineCore.CameraUpdatedEvent.RemoveListener(UpdatePosition);

            this.character = null;

            ReturnToPool();
        }

        private void UpdatePosition(CinemachineBrain brain)
        {
            Vector3 targetWorldPosition = character.transform.position + Vector3.up * 3.0f;
            Vector3 targetScreenPosition = brain.OutputCamera.WorldToScreenPoint(targetWorldPosition);
            targetScreenPosition.z = 0.0f;

            rectTransform.position = targetScreenPosition;
        }

        private void Character_OnCharacterHealthChanged(object sender, Character.OnCharacterHealthChangedArgs args)
        {
            healthBarFillImage.fillAmount = args.HealthPercentage;
        }
    }
}
