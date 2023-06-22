using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Base.UIPackage
{
    public class UILoader : MonoBehaviour
    {
        #region VARIABLES
        #region PUBLIC_VAR
        [Space(20)]
        [Header("References")]
        [Space(20)]
        public Text uiLoaderText;
        #endregion

        #region HIDE_IN_INSPECTOR
        #endregion

        #region PRIVATE_VAR
        Dictionary<string,string> _loaderIdTitle = new Dictionary<string, string>();
        #endregion

        #region ENUMERATOR
        #endregion

        #region STRUCTURE
        #endregion

        #region CLASS
        #endregion

        #region STRUCT
        struct LoadingInformation
        {
            public string id;
            public string title;
        }
        #endregion

        #region GETTER/SETTER
        #endregion

        #region EVENT/DELEGATE
        #endregion
        #endregion

        #region METHODS       
        
        #region UNITY_CALLBACKS
        private void Start()
        {
        }
        #endregion

        #region ON_CLICK_METHODS
        #endregion

        #region SET_METHODS
        public void ShowLoader(string id, string loadingTitle = null, int duration = 0)
        {
            if (_loaderIdTitle.ContainsKey(id))
                return;
            
            gameObject.Open();

            if (string.IsNullOrEmpty(loadingTitle))
                loadingTitle = Constants.Loader.DEFAULT_LOADER_TITLE;

            _loaderIdTitle.Add(id, loadingTitle);
            uiLoaderText.text = _loaderIdTitle.Values?.Last();

            if (duration != 0)
                StartCoroutine(DelayedHideLoader(duration, id));
        }

        public void HideLoader(string id)
        {
            if (_loaderIdTitle.ContainsKey(id))
            {
                _loaderIdTitle.Remove(id);
                if(_loaderIdTitle.Values.Count != 0)
                    uiLoaderText.text = _loaderIdTitle.Values.Last();
            }

            if (_loaderIdTitle.Count == 0)
                HideLoaderForceFully();
        }

        public void HideLoaderForceFully()
        {
            gameObject.Close();
            _loaderIdTitle.Clear();
        }
        #endregion
        #endregion

        #region COROUTINES
        IEnumerator DelayedHideLoader(float delayTime, string id)
        {
            yield return new WaitForSeconds(delayTime);
            HideLoader(id);
        }
        #endregion
    }
}
 