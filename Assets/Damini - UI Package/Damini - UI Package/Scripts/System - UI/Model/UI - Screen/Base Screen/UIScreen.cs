using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platinio;
using System;
using Base.UIPackage.Animtion;

namespace Base.UIPackage
{
    [RequireComponent(typeof(UIAnimator))]
    public class UIScreen : MonoBehaviour
    {
        #region VARIABLES
        #region PUBLIC_VAR
        [Space(20)]
        [Header("References")]
        [Space(20)]
        public ScreenNameContainer screenNameContainer;
        public UIScreen[] dependentSubscreens;
        #endregion

        #region HIDE_IN_INSPECTOR
        #endregion

        #region PRIVATE_VAR
        IUIScreenCallbacks _iUIScreenCallbacks;
        UIAnimator _uIAnimator;
        #endregion

        #region ENUMERATOR
        enum ScreenState { none, opening, opened, closing, closed};
        ScreenState _currentScreenState = ScreenState.none;

        UIManager uIManager;
        #endregion

        #region STRUCTURE
        #endregion

        #region CLASS
        [System.Serializable]
        public struct ScreenNameContainer
        {
            [StringInListAttrib(typeof(ScreenNameContainer), "GetUIScreenData")]
            public string screenName;

            public ScreenNameContainer(string name)
            {
                screenName = name;
            }

            public static List<string> GetUIScreenData()
            {
                return FindObjectOfType<UIManager>().GetUIScreenData();
            }
        }
        #endregion

        #region GETTER/SETTER
        #endregion

        #region EVENT/DELEGATE
        Action _onCloseUICompleteAction;
        #endregion
        #endregion

        #region METHODS
        #region UNITY_CALLBACKS
        public virtual void Awake()
        {
            _iUIScreenCallbacks = GetComponent<IUIScreenCallbacks>();
            _uIAnimator = GetComponent<UIAnimator>();
        }
        #endregion

        #region ON_CLICK_METHODS
        #endregion

        #region SET_METHODS
        public virtual List<string> GetScreenList()
        {
            return null;
        }

        public virtual void Init()
        {
            _currentScreenState = ScreenState.none;
        }

        //<OPEN UI RELATED....
        public virtual void OpenUI()
        {
            Debug.Log(gameObject.name + " OpenUI = " + _currentScreenState);
            //if (_currentScreenState != ScreenState.closed && _currentScreenState != ScreenState.none)
              //  return;

            gameObject.SetActive(true);
            StartCoroutine(CallEventAtEndOfFrame(() =>
            {
                for (int i = 0; i < dependentSubscreens.Length; i++)
                {
                    dependentSubscreens[i].OpenUI();
                }
            }));

            _iUIScreenCallbacks = GetComponent<IUIScreenCallbacks>();
            _currentScreenState = ScreenState.opening;
            _iUIScreenCallbacks?.OpenUIStartCallback();

            AnimationIn();
        }

        void AnimationIn()
        {
            _uIAnimator = GetComponent<UIAnimator>();

            AnimationInDone();
            //float openTime = _uIAnimator.PlayOpenAnimations();
            //Invoke("AnimationInDone", openTime);
        }

        void AnimationInDone()
        {
            OpenUIDone();
        }

        void OpenUIDone()
        {
            _currentScreenState = ScreenState.opened;
            _iUIScreenCallbacks?.OpenUICompleteCallback();
        }
        //>OPEN UI RELATED....

        //<CLOSE UI RELATED....
        public void CloseUI(Action onCloseUICompleteAction)
        {
            Debug.Log(gameObject.name + " CloseUI");

            //if (_currentScreenState != ScreenState.opened)
              //  return;

            _onCloseUICompleteAction = onCloseUICompleteAction;

            for (int i = 0; i < dependentSubscreens.Length; i++)
            {
                dependentSubscreens[i].CloseUI(null);
            }

            _currentScreenState = ScreenState.closing;
            _iUIScreenCallbacks?.CloseUIStartCallback();
            AnimationOutDone();
            //AnimationOut();
        }

        public float GetAnimationCloseTime()
        {
            return _uIAnimator != null ? _uIAnimator.GetAnimationCloseTime() : 0; 
        }

        void AnimationOut()
        {
            Debug.Log(gameObject.name + " AnimationOut");
            float closeTime = _uIAnimator != null ? _uIAnimator.PlayCloseAnimations() : 0;
            float depedentSubscreenTime = 0;

            for (int i = 0; i < dependentSubscreens.Length; i++)
            {
                depedentSubscreenTime = dependentSubscreens[i].GetAnimationCloseTime();
                if (depedentSubscreenTime > closeTime)
                    closeTime = depedentSubscreenTime;
            }


            Debug.Log("closeTime = " + closeTime);
            Debug.Log("closeTime = " + closeTime);

            if (closeTime == 0)
                AnimationOutDone();
            else
                Invoke("AnimationOutDone", closeTime);
        }

        void AnimationOutDone()
        {
            Debug.Log(gameObject.name +  " AnimationOutDone");
            CloseUIDone();
        }

        void CloseUIDone()
        {
            Debug.Log(gameObject.name + " CloseUIDone");

            for (int i = 0; i < dependentSubscreens.Length; i++)
            {
                dependentSubscreens[i].CloseUIDirectly();
            }
            CloseUIDirectly();
            
        }

        public virtual void CloseUIDirectly()
        {
            Debug.Log(gameObject.name + " CloseUIDirectly");

            //if (_currentScreenState == ScreenState.closed)
              //  return;

            _onCloseUICompleteAction?.Invoke();
            _onCloseUICompleteAction = null;
            _iUIScreenCallbacks?.CloseUICompleteCallbak();
            _currentScreenState = ScreenState.closed;

            gameObject.SetActive(false);
        }
        //>CLOSE UI RELATED....
        #endregion
        #endregion

        #region COROUTINES
        IEnumerator CallEventAtEndOfFrame(Action afteEndOfTheFrameAction)
        {
            yield return new WaitForEndOfFrame();
            afteEndOfTheFrameAction?.Invoke();
        }
        #endregion
    }

 
}
