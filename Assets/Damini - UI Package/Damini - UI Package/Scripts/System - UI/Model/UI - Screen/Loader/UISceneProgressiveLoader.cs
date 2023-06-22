using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UIPackage
{
    public class UISceneProgressiveLoader : MonoBehaviour
    {
        #region VARIABLES
        #region PUBLIC_VAR
        [Space(20)]
        [Header("References")]
        [Space(20)]
        public Image fillImage;
        #endregion

        #region HIDE_IN_INSPECTOR
        #endregion

        #region PRIVATE_VAR
        Coroutine _sceneProgressCoroutine;
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
        #endregion

        #region ON_CLICK_METHODS
        #endregion

        #region SET_METHODS
        public void OpenSceneProgressiveLoader(AsyncOperation asyncOperation)
        {
            Debug.Log("OpenSceneProgressiveLoader");
            gameObject.Open();
            if (_sceneProgressCoroutine != null)
                StopCoroutine(_sceneProgressCoroutine);
            _sceneProgressCoroutine = StartCoroutine(SceneAsyncProgressCoroutine(asyncOperation));
        }

        public void HideSceneProgressiveLoader()
        {
            HideSceneProgressiveLoaderForceFully();
        }

        public void HideSceneProgressiveLoaderForceFully()
        {
            gameObject.Close();
        }
        #endregion
        #endregion

        #region COROUTINES
        IEnumerator SceneAsyncProgressCoroutine(AsyncOperation asyncOperation)
        {
            Debug.Log("SceneAsyncProgressCoroutine");
            float fillAmount = 0;
            float loadingSpeed = 5;

            fillImage.fillAmount = fillAmount;
            asyncOperation.allowSceneActivation = false;
            yield return null;

            while (true)
            {
                Debug.Log("asyncOperation.progress = " + asyncOperation.progress);
                fillAmount = asyncOperation.progress;

                if (fillAmount >= 0.9f)
                {
                    //The maximum value of AsyOper.progress is 0.9
                    fillAmount = 1.0f;
                }

                if (fillAmount != fillImage.fillAmount)
                {
                    //Interpolation calculation
                    fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, fillAmount, loadingSpeed * Time.deltaTime);
                    if (Mathf.Abs(fillImage.fillAmount - fillAmount) < 0.01f)
                    {
                        fillImage.fillAmount = fillAmount;
                    }
                }

                if (fillImage.fillAmount == 1.0f)
                {
                    //Allow automatic scene switching after asynchronous loading
                    asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
        #endregion
    }
} 
