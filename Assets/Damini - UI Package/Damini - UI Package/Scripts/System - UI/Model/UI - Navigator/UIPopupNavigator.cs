using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Base.UIPackage
{

    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]

    public class UIPopupNavigator : MonoBehaviour
    {
        public UIPopup nextUIPopup;
        public UIScreen.ScreenNameContainer nextScreenNameContainer;

        public UnityEvent nextPopupAffirmativeCloseAction;
        public UnityEvent nextPopupNegativeCloseAction;


        Button _button;

        private void Awake()
        {
            Debug.Log(gameObject.name + " ...AWAKE...");
            _button = GetComponent<Button>();
            Debug.Log(" ...BUTTON..." + _button);
            _button.onClick.AddListener(ChangeNextScreen);
            Debug.Log(" ........" + _button);
        }

        public void ChangeNextScreen()
        {
            Debug.Log(" ChangeNextScreen" + nextUIPopup);

            UIManager.uIPopupManager.OpenUIPopup(nextScreenNameContainer, ()=> {
                nextPopupAffirmativeCloseAction?.Invoke(); }, () => {
                    nextPopupNegativeCloseAction?.Invoke(); });
        }
    }

   
}