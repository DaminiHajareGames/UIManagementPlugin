using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Base.UIPackage;

[RequireComponent(typeof(UIPopup))]
public abstract class UIPopupCallbacks : MonoBehaviour, IUIScreenCallbacks
{
    public abstract void OpenUICompleteCallback();

    public abstract void OpenUIStartCallback();

    public abstract void CloseUIStartCallback();

    public abstract void CloseUICompleteCallbak();
}



