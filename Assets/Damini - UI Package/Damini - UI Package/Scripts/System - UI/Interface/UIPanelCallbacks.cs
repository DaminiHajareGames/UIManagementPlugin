using UnityEngine;

namespace Base.UIPackage
{
    [RequireComponent(typeof(UIPanel))]
    public abstract class UIPanelCallbacks : MonoBehaviour, IUIScreenCallbacks
    {
        public abstract void OpenUICompleteCallback();

        public abstract void OpenUIStartCallback();

        public abstract void CloseUIStartCallback();

        public abstract void CloseUICompleteCallbak();
    }
}