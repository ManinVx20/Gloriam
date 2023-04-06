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
        protected Image joystickImage;
        [SerializeField]
        protected Image handleImage;
        [Range(0.3f, 0.5f)]
        [SerializeField]
        protected float handleRangeMultiplier = 0.5f;

        protected Vector2 input;
        public Vector2 Input => input;

        private void OnEnable()
        {
            OnPointerUp(null);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                joystickImage.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 position
            ))
            {
                position.x /= joystickImage.rectTransform.sizeDelta.x;
                position.y /= joystickImage.rectTransform.sizeDelta.y;

                Vector2 centerPivot = new Vector2(0.5f, 0.5f);
                position.x += joystickImage.rectTransform.pivot.x - centerPivot.x;
                position.y += joystickImage.rectTransform.pivot.y - centerPivot.y;

                input = Vector2.ClampMagnitude(position, 1.0f);

                float handleAnchoredPositionX = Input.x * joystickImage.rectTransform.sizeDelta.x * handleRangeMultiplier;
                float handleAnchoredPositionY = Input.y * joystickImage.rectTransform.sizeDelta.y * handleRangeMultiplier;

                handleImage.rectTransform.anchoredPosition = new Vector2(
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
            input = Vector2.zero;

            handleImage.rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
