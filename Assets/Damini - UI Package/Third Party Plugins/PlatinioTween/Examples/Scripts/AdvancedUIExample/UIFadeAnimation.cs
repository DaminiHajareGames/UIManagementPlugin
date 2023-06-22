using Platinio.TweenEngine;
using UnityEngine;

namespace Platinio
{
    public class UIFadeAnimation : UIAnimation
    {
        [SerializeField] private CanvasGroup cg = null;
        [SerializeField] private float startAlpha = 0.0f;
        [SerializeField] private float targetAlpha = 1f;

        public override BaseTween Play()
        {
            cg.alpha = startAlpha;
            return cg.Fade(targetAlpha , time).SetEase(ease);
        }


        public override BaseTween PlayReverese()
        {
            cg.alpha = targetAlpha;
            return cg.Fade(startAlpha, time).SetEase(ease);
        }
    }
}

