using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StormDreams
{
    public class SafeZone : MonoBehaviour
    {
        [System.Serializable]
        private struct ShrinkingData
        {
            public float Radius;
            public float GeneratingTime;
            public float ShrinkingTime;
            public float Duration;
        }

        [SerializeField]
        private ShrinkingData[] _shrinkingDataArray;
        [SerializeField]
        private Transform _shrinkingCircleTransform;
        [SerializeField]
        private ColliderTrigger _shrinkingCircleColliderTrigger;
        [SerializeField]
        private Transform _targetCircleTransform;
        [SerializeField]
        private LineRendererCircle _lineRendererCircle;

        private int _shrinkingDataIndex;
        private bool _updated;
        private bool _generated;
        private Utilities.SmoothTransformData _smoothTransformData;
        private float _startRadius;
        private float _endRadius;

        private void Start()
        {
            _shrinkingCircleColliderTrigger.OnCharacterEntered += ShrinkingCircleColliderTrigger_OnCharacterEntered;
            _shrinkingCircleColliderTrigger.OnCharacterExited += ShrinkingCircleColliderTrigger_OnCharacterExited;

            _smoothTransformData.StartLocalPosition = _shrinkingCircleTransform.localPosition;
            _smoothTransformData.EndLocalPosition = _smoothTransformData.StartLocalPosition;
            _smoothTransformData.StartLocalRotation = Quaternion.identity;
            _smoothTransformData.EndLocalRotation = _smoothTransformData.StartLocalRotation;
            _smoothTransformData.StartLocalScale = _shrinkingCircleTransform.localScale;
            _smoothTransformData.EndLocalScale = _smoothTransformData.StartLocalScale;

            _startRadius = _shrinkingCircleTransform.localScale.x * 0.5f;
            _endRadius = _startRadius;

            _targetCircleTransform.gameObject.SetActive(false);

            UIManager.Instance.GetUICanvas<MatchCanvas>().SetShrinkingProgressFillValue(0.0f);
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGamePlaying() && !GameManager.Instance.IsGameOver())
            {
                return;
            }

            if (_shrinkingDataIndex < _shrinkingDataArray.Length)
            {
                float matchTimer = GameManager.Instance.GetGamePlayingTimer();

                if (matchTimer > _shrinkingDataArray[_shrinkingDataIndex].ShrinkingTime && !_updated)
                {
                    _updated = true;
                    _generated = false;

                    UpdateShrinkingCircle(_shrinkingDataArray[_shrinkingDataIndex].Duration, _shrinkingDataIndex == _shrinkingDataArray.Length - 1);

                    _shrinkingDataIndex += 1;
                }
                else if (matchTimer > _shrinkingDataArray[_shrinkingDataIndex].GeneratingTime && !_generated)
                {
                    _generated = true;
                    _updated = false;

                    GenerateNextShrinkingCircle(_shrinkingDataArray[_shrinkingDataIndex].Radius);
                }
            }
        }

        public void GenerateNextShrinkingCircle(float radius)
        {
            _startRadius = _endRadius;
            _endRadius = radius;

            Vector2 randomPointInsideCircle = _endRadius > 0.0f ? Random.insideUnitSphere * (_startRadius - _endRadius) : Vector2.zero;

            _smoothTransformData.StartLocalPosition = _smoothTransformData.EndLocalPosition;
            _smoothTransformData.EndLocalPosition.x += randomPointInsideCircle.x;
            _smoothTransformData.EndLocalPosition.z += randomPointInsideCircle.y;
            _smoothTransformData.StartLocalScale = _smoothTransformData.EndLocalScale;
            _smoothTransformData.EndLocalScale = new Vector3(_endRadius * 2.0f, _smoothTransformData.EndLocalScale.y, _endRadius * 2.0f);

            UIManager.Instance.GetUICanvas<MatchCanvas>().OpenCircleCreatingNotificationUI();

            if (!_targetCircleTransform.gameObject.activeInHierarchy)
            {
                _targetCircleTransform.gameObject.SetActive(true);
                _targetCircleTransform.localPosition = _smoothTransformData.EndLocalPosition;
                _targetCircleTransform.localRotation = _smoothTransformData.EndLocalRotation;
                _targetCircleTransform.localScale = _smoothTransformData.EndLocalScale;

                _lineRendererCircle.DrawCircle(_endRadius, _targetCircleTransform.position);
            }
            else
            {
                UIManager.Instance.GetUICanvas<MatchCanvas>().SetShrinkingProgressFillValue(0.0f);

                StartCoroutine(Utilities.SmoothenTransformCoroutine(_targetCircleTransform, _smoothTransformData, 1.0f, (percentage) =>
                {
                    _lineRendererCircle.DrawCircle(_targetCircleTransform.localScale.x * 0.5f, _targetCircleTransform.position);
                }, () =>
                {
                    if (Mathf.Approximately(_endRadius, 0.0f))
                    {
                        _lineRendererCircle.Clear();
                    }
                }));
            }
        }

        public void UpdateShrinkingCircle(float duration, bool isLastShrinking)
        {
            UIManager.Instance.GetUICanvas<MatchCanvas>().OpenCircleShrinkingNotificationUI();

            StartCoroutine(Utilities.SmoothenTransformCoroutine(_shrinkingCircleTransform, _smoothTransformData, duration, (percentage) =>
            {
                UIManager.Instance.GetUICanvas<MatchCanvas>().SetShrinkingProgressFillValue(percentage);
            }, () =>
            {
                if (isLastShrinking)
                {
                    _shrinkingCircleTransform.position = Vector3.up * 1000.0f;
                }
            }));
        }

        private void ShrinkingCircleColliderTrigger_OnCharacterEntered(object sender, ColliderTrigger.OnCharacterTriggeredEventArgs args)
        {
            args.Character.DisableLoseHealthOverTime();
        }

        private void ShrinkingCircleColliderTrigger_OnCharacterExited(object sender, ColliderTrigger.OnCharacterTriggeredEventArgs args)
        {
            args.Character.EnableLoseHealthOverTime();
        }
    }
}
