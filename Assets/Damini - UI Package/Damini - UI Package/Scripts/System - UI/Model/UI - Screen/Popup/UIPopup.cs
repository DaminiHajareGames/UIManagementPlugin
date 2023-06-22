using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Base.UIPackage
{
    public class UIPopup : UIScreen
    {
        #region VARIABLES
        #region PUBLIC_VAR
        [Space(20)]
        [Header("References")]
        [Space(20)]
        public bool isItAutoClosePopup = false;
        public float autoCloseDelayTime = 0f;
        public List<ButtonAction> affirmativeButtonActionContainers;
        public List<ButtonAction> negativeButtonActionContainers;
        #endregion

        #region HIDE_IN_INSPECTOR
        #endregion

        #region PRIVATE_VAR
        public static List<string> popupList;
        #endregion

        #region ENUMERATOR
        public enum PopupCloseType { negative, affirmative, auto };
        #endregion

        #region STRUCTURE
        #endregion

        #region CLASS

        #endregion

        #region STRUCT
        [System.Serializable]
        public class ButtonAction
        {
            public Button button;
            //public UnityEvent action;
        }
        #endregion

        #region GETTER/SETTER
        #endregion

        #region EVENT/DELEGATE
        public Action<UIScreen.ScreenNameContainer> onOpenPopupAction;
        public Action<UIScreen.ScreenNameContainer> onClosePopupAction;
        Action _affirmativeCloseAction;
        Action _negativeCloseAction;
        #endregion
        #endregion

        #region METHODS
        #region UNITY_CALLBACKS
        public override void Awake()
        {
            SetActionsOnButtons();
        }
        #endregion

        #region ON_CLICK_METHODS
        protected virtual void OnAffirmativeButtonClicked()
        {
            if (isItAutoClosePopup)
                return;

            _affirmativeCloseAction?.Invoke();
            CloseUI(null);
        }

        protected virtual void OnNegativeButtonClicked()
        {
            if (isItAutoClosePopup)
                return;

            _negativeCloseAction?.Invoke();
            CloseUI(null);
        }
        #endregion

        #region SET_METHODS
        void SetActionsOnButtons()
        {
            if (isItAutoClosePopup)
                return;

            for (int i = 0; i < affirmativeButtonActionContainers.Count; i++)
            {
                affirmativeButtonActionContainers[i].button.onClick?.AddListener(() =>
                {
                    //affirmativeButtonActionContainers[i].action?.Invoke();
                    OnAffirmativeButtonClicked();
                });
            }

            for (int i = 0; i < negativeButtonActionContainers.Count; i++)
            {
                int myIndex = i;
                negativeButtonActionContainers[myIndex].button?.onClick.AddListener(() =>
                {
                    //negativeButtonActionContainers[myIndex]?.action?.Invoke();
                    OnNegativeButtonClicked();
                });
            }
        }

        public void OpenUI(Action affirmativeCloseAction, Action negativeCloseAction)
        {
            _affirmativeCloseAction = affirmativeCloseAction;
            _negativeCloseAction = negativeCloseAction;
            OpenUI();
        }

        public override void OpenUI()
        {
            base.OpenUI();
            OpenPopupForUI();
            StartCoroutine(DelayedClosePopup(autoCloseDelayTime));
        }

        public void CloseUI(PopupCloseType popupCloseType)
        {
            switch (popupCloseType)
            {
                case PopupCloseType.negative:
                    OnNegativeButtonClicked();
                    break;
                case PopupCloseType.affirmative:
                    OnAffirmativeButtonClicked();
                    break;
                  default:
                    break;
            }
        }

        void OpenPopupForUI()
        {
            transform.SetAsLastSibling();
            onOpenPopupAction?.Invoke(screenNameContainer);
        }
        
        public override void CloseUIDirectly()
        {
            base.CloseUIDirectly();
            onClosePopupAction?.Invoke(screenNameContainer);
        }

        public override List<string> GetScreenList()
        {
            return popupList;
        }
        #endregion
        #endregion

        #region COROUTINES
        IEnumerator DelayedClosePopup(float delayTime)
        {
            yield return new WaitForSeconds(delayTime);
            CloseUI(PopupCloseType.auto);
        }
        #endregion
    }
}
