using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Base.UIPackage
{
    [RequireComponent(typeof(UIPopupManager))]
    public class UIManager : MonoBehaviour
    {
        #region VARIABLES

        public static UIManager instance;

        #region PUBLIC_VAR
        [Space(20)]
        [Header("UI DATA RELATED")]
        [Space(20)]
        public UIScreenData uIScreenData;

        [Space(20)]
        [Header("UI LOADER RELATED")]
        [Space(20)]
        [SerializeField] UILoader _uILoader;
        [SerializeField] UISceneProgressiveLoader _uISceneProgressiveLoader;

        [Space(20)]
        [Header("UI SCREEN RELATED")]
        [Space(20)]
        [SerializeField] UIScreen.ScreenNameContainer _startUIScreen;
        #endregion

        #region HIDE_IN_INSPECTOR
        [HideInInspector] public static UIPopupManager uIPopupManager;
        #endregion

        #region PRIVATE_VAR
        List<string> uiScreenIds = new List<string>();
        List<UIPanel> _uiScreens = new List<UIPanel>();
        Dictionary<string, UIPanel> _uiPanelDict = new Dictionary<string, UIPanel>();
        UIPanel _previousScreen;
        List<UIScreen> _requestQueue = new List<UIScreen>();
        //static UIScreenData uIScreenDataStatic;
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
        #endregion
        #endregion

        #region METHODS
        #region UNITY_CALLBACKS

        [ContextMenu("Test")]
        public void Test()
        {
            Debug.Log("startUIScreen = " + _startUIScreen.screenName);
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
                Destroy(gameObject);
            }

            uIPopupManager = GetComponent<UIPopupManager>();
            OnAwakeSetUIScreens();
            InitAllUIScreens();
        }

        private void OnEnable()
        {
            InitUI();
        }
        #endregion

        #region ON_CLICK_METHODS
        #endregion

        #region SET_METHODS

        [ContextMenu("SetUIScreenData")]
        public void SetUIScreenData()
        {
           // uIScreenDataStatic = uIScreenData;
        }

        [ContextMenu("CreateEnumForScreenData")]
        public void CreateEnumForScreenData()
        {
//#if UNITY_EDITOR
            EnumBuilder.CreateEnumFromList(SceneManager.GetActiveScene().name + "SceneUIScreensEnum", uIScreenData.uiScreenNamesList);
//#endif
        }

        public List<string> GetUIScreenData()
        {
            //if (uIScreenDataStatic == null)
            //    return null;
            //return uIScreenDataStatic.uiScreenNamesList;

            if (uIScreenData == null)
                return null;
            return uIScreenData.uiScreenNamesList;
        }

        void InitAllUIScreens()
        {
            UIScreen[] uiScreens = FindObjectsOfType<UIScreen>(true);

            for (int i = 0; i < uiScreens.Length; i++)
            {
                uiScreens[i].Init();
            }
        }

        void InitUI()
        {
            StartUIScreen();
            _uILoader.HideLoaderForceFully();
            _uISceneProgressiveLoader.HideSceneProgressiveLoaderForceFully();
        }

        void OnAwakeSetUIScreens()
        {
            GetAllUIScreens();
            SetUIScreenDict();
        }

        void GetAllUIScreens()
        {
            UIPanel[] uiPanles = FindObjectsOfType<UIPanel>(true);

            _uiScreens.Clear();

            for (int i = 0; i < uiPanles.Length; i++)
            {
                _uiScreens.Add(uiPanles[i]);
            }
        } 

        void SetUIScreenDict()
        {
            _uiPanelDict.Clear();
            for (int i = 0; i < _uiScreens.Count; i++)
            {
                Debug.Log(i + _uiScreens[i].screenNameContainer.screenName);
                //if(!_uiPanelDict.ContainsKey(_uiScreens[i].screenNameContainer.screenName))
                    _uiPanelDict.Add(_uiScreens[i].screenNameContainer.screenName, _uiScreens[i]);
            }
        }

        void StartUIScreen() 
        {
            for (int i = 0; i < _uiScreens.Count; i++)
            {
                _uiScreens[i].CloseUIDirectly();
            }

            UIPanel tempUIPanel = _uiPanelDict[_startUIScreen.screenName];

            tempUIPanel?.OpenUI();
            _previousScreen = tempUIPanel;
            uIPopupManager.CloseAllUIPopupsDirectly();
        }

        void ChangeUIScreen(UIPanel uipanel) // enum / class panel / 
        {
            
            uIPopupManager.CloseAllUIPopupsDirectly();

            _previousScreen.CloseUI(null);
            uipanel.OpenUI();
            _previousScreen = uipanel;

            //bool previousScreenConnectedWithNew = false;
            //for (int i = 0; i < uipanel.dependentSubscreens.Length; i++)
            //{
            //    if (uipanel.dependentSubscreens.Equals(_previousScreen))
            //        previousScreenConnectedWithNew = true;
            //}

            //if (!previousScreenConnectedWithNew)
            //{
            //    _previousScreen.CloseUI(() =>
            //    {
            //            //CommanOfDamini.CommanForAllGame.instance.CallEventAtEndOfFrame(() =>
            //            //{
            //            uipanel.OpenUI();
            //        _previousScreen = uipanel;
            //            //});
            //        });
            //}
            //else
            //{
            //    uipanel.OpenUI();
            //    _previousScreen = uipanel;
            //}

        }


        //void ChangeUIScreen(UIPanel uipanel) // enum / class panel / 
        //{
        //    uIPopupManager.CloseAllUIPopups(() =>
        //    {
        //    bool previousScreenConnectedWithNew = false;
        //        for (int i = 0; i < uipanel.dependentSubscreens.Length; i++)
        //        {
        //            if (uipanel.dependentSubscreens.Equals(_previousScreen))
        //                previousScreenConnectedWithNew = true;
        //        }
        //
        //        if (!previousScreenConnectedWithNew)
        //        {
        //            _previousScreen.CloseUI(() =>
        //            {
        //            //CommanOfDamini.CommanForAllGame.instance.CallEventAtEndOfFrame(() =>
        //            //{
        //                uipanel.OpenUI();
        //                _previousScreen = uipanel;
        //            //});
        //        });
        //        }
        //        else
        //        {
        //            uipanel.OpenUI();
        //            _previousScreen = uipanel;
        //        }
        //    });
        //}

        public void ChangeUIScreen(UIScreen.ScreenNameContainer screen) // enum / class panel / 
        {
            ChangeUIScreen(_uiPanelDict[screen.screenName]);
        }

        public void ChangeUIScreen(string screen) // enum / class panel / 
        {
            ChangeUIScreen(_uiPanelDict[screen]);
        }


        public void OpenLoader(string screen, string loadingTitle = null, int duration = 0)
        {
            _uILoader.ShowLoader(screen, loadingTitle, duration);
        }

        public void HideLoader(string screen)
        {
            _uILoader.HideLoader(screen);
        }

        public void OpenSceneProgressiveLoader(AsyncOperation sceneAsyncOperation)
        {
            _uISceneProgressiveLoader.OpenSceneProgressiveLoader(sceneAsyncOperation);
        }
        #endregion
        #endregion

        #region COROUTINES
        #endregion
    }
}