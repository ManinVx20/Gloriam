using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StormDreams
{
    public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        protected Image _joystickImage;
        [SerializeField]
        protected Image _handleImage;
        [Range(0.3f, 0.5f)]
        [SerializeField]
        protected float _handleRangeMultiplier = 0.5f;

        protected Vector2 _input;
        public Vector2 Input => _input;

        private void OnEnable()
        {
            OnPointerUp(null);
        }

        private void OnDisable()
        {
            OnPointerUp(null);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _joystickImage.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 position
            ))
            {
                position.x /= _joystickImage.rectTransform.sizeDelta.x;
                position.y /= _joystickImage.rectTransform.sizeDelta.y;

                Vector2 centerPivot = new Vector2(0.5f, 0.5f);
                position.x += _joystickImage.rectTransform.pivot.x - centerPivot.x;
                position.y += _joystickImage.rectTransform.pivot.y - centerPivot.y;

                _input = Vector2.ClampMagnitude(position, 1.0f);

                float handleAnchoredPositionX = Input.x * _joystickImage.rectTransform.sizeDelta.x * _handleRangeMultiplier;
                float handleAnchoredPositionY = Input.y * _joystickImage.rectTransform.sizeDelta.y * _handleRangeMultiplier;

                _handleImage.rectTransform.anchoredPosition = new Vector2(
                    handleAnchoredPositionX,
                    handleAnchoredPositionY
                );
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            _input = Vector2.zero;

            _handleImage.rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
