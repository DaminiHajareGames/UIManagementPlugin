using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

namespace Base.UIPackage {
    public class UIDynamicPopup : UIPopup
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI messageText;
        [SerializeField] TextMeshProUGUI okButtonText;
        [SerializeField] TextMeshProUGUI affirmativeButtonText;
        [SerializeField] TextMeshProUGUI negativeButtonText;

        [SerializeField] Button alertButton;
        [SerializeField] Button okButton;
        [SerializeField] Button yesButton;
        [SerializeField] Button noButton;

        Action okButtonAction;
        Action affirmativeButtonAction;    
        Action negativeButtonAction;

        public override void Awake()
        {
            base.Awake();
            alertButton.onClick.AddListener(OnAlertButtonClicked);
            okButton.onClick.AddListener(OnOkButtonClicked);
            yesButton.onClick.AddListener(OnYesButtonClicked);
            noButton.onClick.AddListener(OnNoButtonClicked);
        }

        void OnYesButtonClicked()
        {
            CloseUI(null);
            affirmativeButtonAction?.Invoke();
        }

        void OnNoButtonClicked()
        {
            CloseUI(null);
            negativeButtonAction?.Invoke();
        }

        void OnAlertButtonClicked()
        {
            CloseUI(null);
        }

        void OnOkButtonClicked()
        {
            CloseUI(null);
            okButtonAction?.Invoke();
        }


        public void OpenDynamicInformativePopup(string title, string message, Action okButtonAction = null, string okButtonString = null)
        {
            CloseAllbuttons();

            okButton.gameObject.Open();

            this.okButtonAction = okButtonAction;

            if (!string.IsNullOrEmpty(okButtonString))
                okButtonText.text = okButtonString;
            else
                okButtonText.text = "Okay.";

            OpenUI(title, message);
        }

        public void OpenDynamicAffirmativePopup(string title, string message, Action affirmativeButtonAction = null, Action negativeButtonAction = null, string affirmativeButtonString = null, string negativeButtonString = null)
        {
            CloseAllbuttons();
            
            yesButton.gameObject.Open();
            noButton.gameObject.Open();
            
            this.affirmativeButtonAction = affirmativeButtonAction; 
            this.negativeButtonAction = negativeButtonAction;
            
            
            if (!string.IsNullOrEmpty(affirmativeButtonString))
                affirmativeButtonText.text = affirmativeButtonString;
            else
                affirmativeButtonText.text = "Yes.";
            
            if (!string.IsNullOrEmpty(negativeButtonString))
                negativeButtonText.text = negativeButtonString;
            else
                negativeButtonText.text = "No.";
            
            OpenUI(title, message);
        }

        public void OpenDynamicAlertPopup(string title, string message)
        {
            CloseAllbuttons();

            alertButton.gameObject.Open();
            OpenUI(title, message);
        }

        void CloseAllbuttons()
        {
            yesButton.gameObject.Close();
            noButton.gameObject.Close();
            alertButton.gameObject.Close(); 
            okButton.gameObject.Close();
        }

        void OpenUI(string title, string message)
        {
            base.OpenUI();
            titleText.text = title;
            messageText.text = message;
        }
    }
}
