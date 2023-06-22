using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Base.UIPackage
{
    [RequireComponent(typeof(UIManager))]
    public class UIPopupManager : MonoBehaviour
    {
        #region VARIABLES
        #region PUBLIC_VAR
        #endregion

        #region HIDE_IN_INSPECTOR
        [HideInInspector] public static UIDynamicPopup uIDynamicPopup;
        #endregion

        #region PRIVATE_VAR
        List<UIPopup> _uIPopups = new List<UIPopup>();
        List<string> _uiPopupStack = new List<string>();
        Dictionary<string, UIPopup> _popUpDict = new Dictionary<string, UIPopup>();
        #endregion                                      

        #region ENUMERATOR
        #endregion

        #region STRUCTURE
        #endregion

        #region CLASS
        #endregion

        #region GETTER/SETTER
        #endregion

        #region EVENT/DELEGATE
        public delegate void PopupAction(string popupId);

        public event PopupAction PopupOpenEvent;
        public event PopupAction PopupCloseEvent;                      
        #endregion
        #endregion

        #region METHODS
        #region UNITY_CALLBACKS
        private void Awake()
        {
            OnAwakeSetUIScreens();
            uIDynamicPopup = FindObjectOfType<UIDynamicPopup>(true);
        }

        private void OnEnable()
        {
            InitUI();
            SubscribeToAllPopups();
        }

        private void OnDisable()
        {
            UnSubscribeToAllPopups();
        }
        #endregion

        #region ON_CLICK_METHODS
        #endregion

        #region SET_METHODS
        void OnAwakeSetUIScreens()
        {
            GetAllUIScreens();
            SetPopupDict();
        }

        void GetAllUIScreens()
        {
            UIPopup[] uiPanles = FindObjectsOfType<UIPopup>(true);

            _uIPopups.Clear();

            for (int i = 0; i < uiPanles.Length; i++)
            {
                _uIPopups.Add(uiPanles[i]);
            }
        }

        void SetPopupDict()
        {
            _popUpDict.Clear();
            for (int i = 0; i < _uIPopups.Count; i++)
            {
                _popUpDict.Add(_uIPopups[i].screenNameContainer.screenName, _uIPopups[i]);
            }
        }

        void SubscribeToAllPopups()
        {
            for (int i = 0; i < _uIPopups.Count; i++)
            {
                _uIPopups[i].onOpenPopupAction += OnOpenUIPopup;
                _uIPopups[i].onClosePopupAction += OnCloseUIPopup;
            }
        }

        void UnSubscribeToAllPopups()
        {
            for (int i = 0; i < _uIPopups.Count; i++)
            {
                _uIPopups[i].onOpenPopupAction -= OnOpenUIPopup;
                _uIPopups[i].onClosePopupAction -= OnCloseUIPopup;
            }
        }

        void InitUI()
        {
            for (int i = 0; i < _uIPopups.Count; i++)
            {
                _uIPopups[i].CloseUIDirectly();
            }
        }

        void AddPopupIntoStack(string popupId)
        {
            if (_uiPopupStack.Contains(popupId))
            {
                _uiPopupStack.Remove(popupId);
            }
            _uiPopupStack.Add(popupId);
        }

        void RemovePopupFromStack(string popupId)
        {
            _uiPopupStack.Remove(popupId);
        }

        public void OpenUIPopup(string popupId, Action affirmativeCloseAction = null, Action negativeCloseAction = null)
        {
            _popUpDict[popupId].OpenUI(affirmativeCloseAction, negativeCloseAction);
        }

        public void OpenUIPopup(UIScreen.ScreenNameContainer popup, Action affirmativeCloseAction, Action negativeCloseAction)
        {
            _popUpDict[popup.screenName].OpenUI(affirmativeCloseAction, negativeCloseAction);
        }

        public void CloseUIPopup(string popupId, UIPopup.PopupCloseType popupCloseType)
        {
            _popUpDict[popupId].CloseUI(popupCloseType);
        }

        void OnOpenUIPopup(UIScreen.ScreenNameContainer popupId)
        {
            AddPopupIntoStack(popupId.screenName);
        }

        void OnCloseUIPopup(UIScreen.ScreenNameContainer popupId)
        {
            RemovePopupFromStack(popupId.screenName);
        }

        public void CloseAllUIPopups(Action allUIPopupsClosed)
        {
            Debug.Log("CloseAllUIPopups");
            //StartCoroutine(CloseAllUIPopupsWithDelay(allUIPopupsClosed));
            CloseAllUIPopupsDirectly();
        }

        public void CloseAllUIPopupsDirectly()
        {
            Debug.Log("CloseAllUIPopupsDirectly");

            int count = _uiPopupStack.Count;

            for (int i = 0; i < count; i++)
            {
                if (_popUpDict.ContainsKey(_uiPopupStack[0]))
                {
                    _popUpDict[_uiPopupStack[0]].CloseUIDirectly();
                }
            }
        }
        #endregion
        #endregion

        #region COROUTINES
        IEnumerator CloseAllUIPopupsWithDelay(Action allUIPopupsClosed)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.2f);

            int count = _uiPopupStack.Count;

            for (int i = 0; i < count; i++)
            {
                if (_popUpDict.ContainsKey(_uiPopupStack[0]))
                {
                    _popUpDict[_uiPopupStack[0]].CloseUI(null);
                    yield return waitForSeconds;
                }
            }

            allUIPopupsClosed?.Invoke();
        }
        #endregion
    }
} 