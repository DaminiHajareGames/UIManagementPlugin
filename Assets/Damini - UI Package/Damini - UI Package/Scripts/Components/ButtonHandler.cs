using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Base.UIPackage.Component
{
    public class ButtonHandler : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [Tooltip("While button pressing continue")]
        public UnityEvent eventOnButtonPressed;
        [Tooltip("Only on button pressed")]
        public UnityEvent eventOnlyOnButtonPressed;
        public UnityEvent eventOnButtonReleased;

        public UnityEvent longPressedButtonEvent;
        public float waitDurationForLongPress;

        Button _button;
        Slider _slider;

        bool isButtonPressed = false;

        void Start()
        {
            _button = GetComponent<Button>();
            _slider = GetComponent<Slider>();
        }

        bool CheckIfInteractable()
        {
            if (_button != null)
            {
                if (_button.interactable)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (_slider != null)
            {
                if (_slider.interactable)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isButtonPressed = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!CheckIfInteractable())
            {
                return;
            }
            isButtonPressed = true;
            StartCoroutine(ButtonPressed());
        }

        IEnumerator ButtonPressed()
        {
            float startTime = Time.time;
            bool longPressedCalled = false;
            while (true)
            {
                if (!isButtonPressed || !CheckIfInteractable())
                {
                    break;
                }
                eventOnButtonPressed?.Invoke();

                if (!longPressedCalled && Time.time - startTime > waitDurationForLongPress)
                {
                    longPressedButtonEvent?.Invoke();
                    longPressedCalled = true;
                }
                yield return null;
            }

            if (!longPressedCalled)
                eventOnlyOnButtonPressed?.Invoke();

            eventOnButtonReleased?.Invoke();
        }
    }
}

