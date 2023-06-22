using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Base.UIPackage {

    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]

    public class UIPanelNavigator : MonoBehaviour
    {
        public UIScreen.ScreenNameContainer nextUIPanelContainer;
        public UnityEvent beforePageOpenEvents;
        public UnityEvent afterPageOpenEvents;

        Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ChangeNextScreen);
        }

        public void ChangeNextScreen()
        {
            beforePageOpenEvents?.Invoke();
            UIManager.instance.ChangeUIScreen(nextUIPanelContainer);
            afterPageOpenEvents?.Invoke();
        }
    }
}