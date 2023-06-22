using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Base.UIPackage
{
    public class UIProgressBar : MonoBehaviour
    {
        #region VARIABLES
        #region PUBLIC_VAR
        [Space(20)]
        [Header("References")]
        [Space(20)]
        [SerializeField] Image fillingImage;
        #endregion

        #region HIDE_IN_INSPECTOR
        #endregion

        #region PRIVATE_VAR
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
        public void SetStartValues(int startValue)
        {

        }

        void LerpProgressbarInDuration(int startValue, int targetValue, float duration)
        {

        }

        void LerpProgresbarInPercentage(float startPercentage, float targetPercentage)
        {

        }
        #endregion
        #endregion

        #region COROUTINES
        #endregion
    }
}