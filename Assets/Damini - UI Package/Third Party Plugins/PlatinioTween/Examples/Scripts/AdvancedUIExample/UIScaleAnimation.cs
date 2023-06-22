using Platinio.TweenEngine;
using UnityEngine;

namespace Platinio
{
    public class UIScaleAnimation : UIAnimation
    {
        [SerializeField] AnimationFields inAnimation;
        [SerializeField] AnimationFields outAnimation;

        [System.Serializable]
        class AnimationFields
        {
            public Vector3 startScale = Vector3.zero;
            public Vector3 targetScale = Vector3.one;
        }

        public override BaseTween Play()
        {
            Debug.Log("Play");
            transform.localScale = inAnimation.startScale;
            return gameObject.ScaleTween(inAnimation.targetScale, time).SetEase(ease);
        }

        public override BaseTween PlayReverese()
        {
            transform.localScale = outAnimation.targetScale;
            return gameObject.ScaleTween(outAnimation.startScale, time).SetEase(ease);
        }
    }
}

