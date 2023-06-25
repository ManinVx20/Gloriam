using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace StormDreams
{
    public class CharacterInfoUI : PoolableObject
    {
        [SerializeField]
        private TextMeshProUGUI _nameText;
        [SerializeField]
        private Image _healthBarFillImage;
        [SerializeField]
        private Image _energyBarFillImage;

        private RectTransform _rectTransform;
        private Character _character;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public override void Dispose()
        {
            _character.OnCharacterHealthChanged -= Character_OnCharacterHealthChanged;
            _character.OnCharacterEnergyChanged -= Character_OnCharacterEnergyChanged;

            CinemachineCore.CameraUpdatedEvent.RemoveListener(UpdatePosition);

            base.Dispose();
        }

        public void Initialize(Character character)
        {
            _character = character;

            CinemachineCore.CameraUpdatedEvent.AddListener(UpdatePosition);

            _character.OnCharacterHealthChanged += Character_OnCharacterHealthChanged;
            _character.OnCharacterEnergyChanged += Character_OnCharacterEnergyChanged;
        }

        public void Despawn()
        {
            _character.OnCharacterHealthChanged -= Character_OnCharacterHealthChanged;
            _character.OnCharacterEnergyChanged -= Character_OnCharacterEnergyChanged;

            CinemachineCore.CameraUpdatedEvent.RemoveListener(UpdatePosition);

            _character = null;

            ReturnToPool();
        }

        private void UpdatePosition(CinemachineBrain brain)
        {
            Vector3 targetWorldPosition = _character.transform.position + Vector3.up * 3.0f;
            Vector3 targetScreenPosition = brain.OutputCamera.WorldToScreenPoint(targetWorldPosition);
            targetScreenPosition.z = 0.0f;

            _rectTransform.position = targetScreenPosition;
        }

        private void Character_OnCharacterHealthChanged(object sender, Character.OnCharacterHealthChangedArgs args)
        {
            _healthBarFillImage.fillAmount = args.HealthPercentage;
        }

        private void Character_OnCharacterEnergyChanged(object sender, Character.OnCharacterEnergyChangedArgs args)
        {
            _energyBarFillImage.fillAmount = args.EnergyPercentage;
        }
    }
}
