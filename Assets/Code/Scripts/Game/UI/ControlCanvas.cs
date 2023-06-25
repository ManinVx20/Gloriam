using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StormDreams
{
    public class ControlCanvas : UICanvas
    {
        [SerializeField]
        private Toggle _attackAutoToggle;

        private void Awake()
        {
            _attackAutoToggle.onValueChanged.AddListener((value) =>
            {
                Player.Instance.ToggleAttackAuto();
            });
        }

        public override void Setup()
        {
            base.Setup();

            _attackAutoToggle.isOn = false;
        }
    }
}
