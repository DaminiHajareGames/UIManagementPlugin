using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Platinio.UI;
using System;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Base.UIPackage.Animtion
{
    public enum AnimationType { Translate, Rotate, Scalling, Fading }

    public class UIOpenCloseAnim : MonoBehaviour
    {
        [SerializeField] UIAnimationBundle openMotion;
        [SerializeField] UIAnimationBundle closeMotion;

        private void Awake()
        {
            openMotion.SetProperties(this);
            closeMotion.SetProperties(this);
        }

        public float GetOpenPlayTime()
        {
            return openMotion.GetPlayTime();
        }

        public float GetClosePlayTime()
        {
            return closeMotion.GetPlayTime();
        }

        public float PlayOpenMotion()
        {
            openMotion.StopLoopModeAnimation(AnimationType.Translate);
            openMotion.StopLoopModeAnimation(AnimationType.Scalling);
            openMotion.StopLoopModeAnimation(AnimationType.Rotate);
            openMotion.StopLoopModeAnimation(AnimationType.Fading);
            return openMotion.Play();
        }

        public void SetValuesBeforePlayForOpen()
        {
            openMotion.SetStartValuesBeforePlay();
        }

        public float PlayCloseMotion()
        {
            openMotion.StopLoopModeAnimation(AnimationType.Translate);
            openMotion.StopLoopModeAnimation(AnimationType.Scalling);
            openMotion.StopLoopModeAnimation(AnimationType.Rotate);
            openMotion.StopLoopModeAnimation(AnimationType.Fading);

            return closeMotion.Play();
        }

        public void StopLoopModeAnimation(AnimationType animationType)
        {
            openMotion.StopLoopModeAnimation(animationType);
        }

        public void SetValuesBeforePlayForClose()
        {
            closeMotion.SetStartValuesBeforePlay();
        }
    }


    [System.Serializable]
    class UIAnimationBundle
    {
        [SerializeField] MoveAnimation moveAnimation;
        [SerializeField] RotationEffect rotationEffect;
        [SerializeField] ScaleEffect scaleEffect;
        [SerializeField] AlphaEffect alphaEffect;

        public void SetStartValuesBeforePlay() {
            moveAnimation.SetStartValuesBeforePlay();
            rotationEffect.SetStartValuesBeforePlay();
            scaleEffect.SetStartValuesBeforePlay();
            alphaEffect.SetStartValuesBeforePlay();
        }

        public void SetProperties(MonoBehaviour monoBehaviour)
        {
            moveAnimation.SetProperties(monoBehaviour);
            rotationEffect.SetProperties(monoBehaviour);
            scaleEffect.SetProperties(monoBehaviour);
            alphaEffect.SetProperties(monoBehaviour);
        }

        public float GetPlayTime()
        {
            List<float> animationTimeArray = new List<float>();

            animationTimeArray.Add(moveAnimation.GetPlayTime());
            animationTimeArray.Add(rotationEffect.GetPlayTime());
            animationTimeArray.Add(scaleEffect.GetPlayTime());
            animationTimeArray.Add(alphaEffect.GetPlayTime());

            float maxTime = animationTimeArray[0];

            for (int i = 1; i < animationTimeArray.Count; i++)
            {
                if (animationTimeArray[i] > maxTime)
                    maxTime = animationTimeArray[i];
            }

            return maxTime;
        }

        public float Play()
        {
            moveAnimation.Play();
            rotationEffect.Play();
            scaleEffect.Play();
            alphaEffect.Play();

            return GetPlayTime();
        }

        public void StopLoopModeAnimation(AnimationType animationType)
        {
            if (animationType != AnimationType.Scalling)
            {
                return;
            }

            switch (animationType)
            {
                case AnimationType.Translate:
                    moveAnimation.StopLoopMode();
                    break;
                case AnimationType.Rotate:
                    rotationEffect.StopLoopMode();
                    break;
                case AnimationType.Scalling:
                    scaleEffect.StopLoopMode();
                    break;
                case AnimationType.Fading:
                    alphaEffect.StopLoopMode();
                    break;
                default:
                    break;
            }
        }
    }

    [System.Serializable]
    abstract class UIAnimation
    {
        [SerializeField] public bool enableEffect;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] protected float delayToStart;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] protected bool inLoopMode = false;


        [EnableIf(EConditionOperator.And, "enableEffect", "revereseInLoopMode")] [AllowNesting] [SerializeField] protected AnimationParameters animationParameters;

        bool revereseInLoopMode => !inLoopMode;

        [EnableIf(EConditionOperator.And, "enableEffect", "inLoopMode")] [AllowNesting] [SerializeField] protected int loopCount = 0;
        [EnableIf(EConditionOperator.And, "enableEffect", "inLoopMode")] [AllowNesting] [SerializeField] protected AnimationParameters loopInAnimParameters;
        [EnableIf(EConditionOperator.And, "enableEffect", "inLoopMode")] [AllowNesting] [SerializeField] protected AnimationParameters loopOutAnimParameters;

        protected MonoBehaviour monoBehaviour = null;

        Sequence _currentPlayingSequence;

        public enum AnimationValueType { CustomValue, StartValue, CurrentValue };

        [System.Serializable]
        public struct AnimationParameters
        {
            public DG.Tweening.Ease easeType;
            public float easeTime;
            public float delayAfterStop;

            public float GetPlayTime()
            {
                return easeTime + delayAfterStop;
            }
        }

        public virtual void SetProperties(MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour;
            SaveStartTimeValue();
        }

        public float GetPlayTime()
        {
            if (enableEffect)
            {
                if (loopCount == 0)
                    return animationParameters.GetPlayTime() + delayToStart;
                else if (loopCount != 1)
                    return ((loopInAnimParameters.GetPlayTime() + loopOutAnimParameters.GetPlayTime()) * loopCount) + delayToStart;
                else
                    return (loopInAnimParameters.GetPlayTime() + loopOutAnimParameters.GetPlayTime()) + delayToStart; //As it is infinite loop. Consider first time parameters only.
            }
            else
                return 0;
        }

        public float Play()
        {
            if (enableEffect)
            {
                SetStartValuesBeforePlay();
                _currentPlayingSequence = PerformPlayAnim();
                StopLoopMode();
            }
            return GetPlayTime();
        }

        public abstract void SetStartValuesBeforePlay();

        protected abstract Sequence PerformPlayAnim();

        public virtual void StopLoopMode()
        {
            if (loopCount == 0)
                return;

            _currentPlayingSequence?.Complete();
        }
        
        public abstract void SaveStartTimeValue();
    }

    [System.Serializable]
    class MoveAnimation : UIAnimation
    {
        bool _showStartCustomValue => startValueType == AnimationValueType.CustomValue;
        bool _showTargetCustomValue => targetValueType == AnimationValueType.CustomValue;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] AnimationValueType startValueType;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showStartCustomValue")] [AllowNesting] [SerializeField] Vector2 startPos;
        [EnableIf(EConditionOperator.And,"enableEffect", "_showStartCustomValue")] [AllowNesting] [SerializeField] RectTransform startPosTransform;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] AnimationValueType targetValueType;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showTargetCustomValue")] [AllowNesting] [SerializeField] Vector2 targetPos;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showTargetCustomValue")] [AllowNesting] [SerializeField] RectTransform targetPosTransform;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] private RectTransform canvas = null;

        RectTransform _rectTransform;
        Vector3 _startTimeValue;

        Vector3 _currentStartValue;
        Vector3 _currentTargetValue;

        public override void SetProperties(MonoBehaviour monoBehaviour)
        {
            base.SetProperties(monoBehaviour);
            _rectTransform = monoBehaviour.GetComponent<RectTransform>();

        }

        public Vector3 GetAnimationTypeWiseValue(AnimationValueType animationValueType)
        {
            switch (startValueType)
            {
                case AnimationValueType.StartValue:
                    return GetStartTimeValue();
                case AnimationValueType.CurrentValue:
                    return GetCurrentValue();
            }
            return Vector3.zero;
        }  

        public override void SetStartValuesBeforePlay()
        {
            if (!enableEffect)
            {
                return;
            }

            if (startValueType == AnimationValueType.CustomValue)
            {
                if (startPosTransform != null)
                    monoBehaviour.transform.position = startPosTransform.position;
                else
                    _rectTransform.anchoredPosition = _rectTransform.FromAbsolutePositionToAnchoredPosition(startPos, canvas);
            }
            else
                monoBehaviour.transform.position = GetAnimationTypeWiseValue(startValueType);
        }

        protected override Sequence PerformPlayAnim()
        {
            Sequence s = DOTween.Sequence();
            s.SetDelay(delayToStart, false);

            if (targetPosTransform != null)
            {
                if (loopCount == 0)
                {
                    s.Append(_rectTransform.DOMove(targetPosTransform.position, animationParameters.easeTime, true).SetEase(animationParameters.easeType).SetDelay(animationParameters.delayAfterStop));
                }
                else
                {
                    s.Append(_rectTransform.DOMove(targetPosTransform.position, loopInAnimParameters.easeTime, true).SetDelay(loopInAnimParameters.delayAfterStop).SetEase(loopInAnimParameters.easeType));
                    if(startPosTransform != null)
                        s.Append(_rectTransform.DOMove(startPosTransform.position, loopOutAnimParameters.easeTime, true).SetDelay(loopOutAnimParameters.delayAfterStop).SetEase(loopOutAnimParameters.easeType));
                    else
                        s.Append(_rectTransform.DOAnchorPos(startPos, loopOutAnimParameters.easeTime, true).SetDelay(loopOutAnimParameters.delayAfterStop).SetEase(loopOutAnimParameters.easeType));

                    s.SetLoops(loopCount, LoopType.Restart);
                }
            }
            else
            {
                if (loopCount == 0)
                {
                    s.Append(_rectTransform.DOAnchorPos(targetPos, animationParameters.easeTime, true).SetEase(animationParameters.easeType));
                }
                else
                {
                    s.Append(_rectTransform.DOAnchorPos(targetPos, loopInAnimParameters.easeTime, true).SetDelay(loopInAnimParameters.delayAfterStop).SetEase(loopInAnimParameters.easeType));

                    if (startPosTransform != null)
                        s.Append(_rectTransform.DOMove(startPosTransform.position, loopOutAnimParameters.easeTime, true).SetDelay(loopOutAnimParameters.delayAfterStop).SetEase(loopOutAnimParameters.easeType));
                    else
                        s.Append(_rectTransform.DOAnchorPos(startPos, loopOutAnimParameters.easeTime, true).SetDelay(loopOutAnimParameters.delayAfterStop).SetEase(loopOutAnimParameters.easeType));

                    s.Append(_rectTransform.DOAnchorPos(targetPosTransform.position, loopOutAnimParameters.easeTime, true).SetDelay(loopOutAnimParameters.delayAfterStop).SetEase(loopOutAnimParameters.easeType));
                    s.SetLoops(loopCount, LoopType.Restart);
                }
            }

            return s;
        }

        public override void SaveStartTimeValue()
        {
            //_startTimeValue = _rectTransform.position;
        }

        public Vector3 GetCurrentValue()
        {
            return _rectTransform.position;
        }

        public Vector3 GetStartTimeValue()
        {
            return _startTimeValue;
        }
    }

    [System.Serializable]
    class RotationEffect : UIAnimation
    {
        bool _showStartCustomValue => startValueType == AnimationValueType.CustomValue;
        bool _showTargetCustomValue => targetValueType == AnimationValueType.CustomValue;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] AnimationValueType startValueType;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showStartCustomValue")] [AllowNesting] [SerializeField] Vector3 startRotation;
        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] AnimationValueType targetValueType;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showTargetCustomValue")] [AllowNesting] [SerializeField] Vector3 targetRotation;

        Vector3 _startTimeValue;

        Vector3 _currentStartValue;
        Vector3 _currentTargetValue;

        GameObject _obj;

        public override void SetProperties(MonoBehaviour monoBehaviour)
        {
            base.SetProperties(monoBehaviour);
            _obj = monoBehaviour.gameObject;
        }

        public override void SetStartValuesBeforePlay()
        {
            if (!enableEffect)
            {
                return;
            }
            _currentStartValue = GetAnimationTypeWiseValue(startValueType, startRotation);
            _obj.transform.eulerAngles = _currentStartValue;
        }

        protected override Sequence PerformPlayAnim()
        {
            SetStartValuesBeforePlay();

            _currentStartValue = GetAnimationTypeWiseValue(startValueType, startRotation);
            _currentTargetValue = GetAnimationTypeWiseValue(targetValueType, targetRotation);


            Sequence s = DOTween.Sequence();
            s.SetDelay(delayToStart, false);

            if (loopCount == 0)
            {
                s.Append(_obj.transform.DORotate(_currentTargetValue, animationParameters.easeTime, RotateMode.FastBeyond360).SetEase(animationParameters.easeType));
            }
            else
            {
                s.Append(_obj.transform.DORotate(_currentTargetValue, loopInAnimParameters.easeTime, RotateMode.FastBeyond360).SetDelay(loopInAnimParameters.delayAfterStop).SetEase(loopInAnimParameters.easeType));
                s.Append(_obj.transform.DORotate(_currentStartValue, loopOutAnimParameters.easeTime, RotateMode.FastBeyond360).SetDelay(loopOutAnimParameters.delayAfterStop).SetEase(loopOutAnimParameters.easeType));
                s.SetLoops(loopCount, LoopType.Restart);
            }

            return s;
        }

        public Vector3 GetAnimationTypeWiseValue(AnimationValueType animationValueType, Vector3 customValue)
        {
            switch (startValueType)
            {
                case AnimationValueType.CustomValue:
                    return customValue;
                case AnimationValueType.StartValue:
                    return GetStartTimeValue();
                case AnimationValueType.CurrentValue:
                    return GetCurrentValue();
            }
            return customValue;
        }

        public override void SaveStartTimeValue()
        {
            //_startTimeValue = _obj.transform.eulerAngles;
        }

        public Vector3 GetStartTimeValue()
        {
            return _startTimeValue;
        }

        public Vector3 GetCurrentValue()
        {
            return _obj.transform.eulerAngles; 
        }
    }

    [System.Serializable]
    class ScaleEffect : UIAnimation
    {
        bool _showStartCustomValue => startValueType == AnimationValueType.CustomValue;
        bool _showTargetCustomValue => targetValueType == AnimationValueType.CustomValue;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] AnimationValueType startValueType;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showStartCustomValue")] [AllowNesting] [SerializeField] Vector3 startScale;
        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] AnimationValueType targetValueType;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showTargetCustomValue")] [AllowNesting] [SerializeField] Vector3 targetScale;

        Vector3 _currentStartScale;
        Vector3 _currentTargetScale;

        Vector3 _startTimeValue;
        GameObject _obj;

        public override void SetProperties(MonoBehaviour monoBehaviour)
        {
            base.SetProperties(monoBehaviour);
            _obj = monoBehaviour.gameObject;
        }

        public override void SetStartValuesBeforePlay()
        {
            if (!enableEffect)
            {
                return;
            }
            _currentStartScale = GetAnimationTypeWiseValue(startValueType, startScale);
            _obj.transform.localScale = _currentStartScale;
        }

        protected override Sequence PerformPlayAnim()
        {
            _currentStartScale = GetAnimationTypeWiseValue(startValueType, startScale);
            _currentTargetScale = GetAnimationTypeWiseValue(targetValueType, targetScale);

            Sequence s = DOTween.Sequence();
            s.SetDelay(delayToStart, false);

            if (loopCount == 0)
            {
                s.Append(_obj.transform.DOScale(_currentTargetScale, animationParameters.easeTime).SetEase(animationParameters.easeType));
            }
            else
            {
                s.Append(_obj.transform.DOScale(_currentTargetScale, loopInAnimParameters.easeTime).SetDelay(loopInAnimParameters.delayAfterStop).SetEase(loopInAnimParameters.easeType));
                s.Append(_obj.transform.DOScale(_currentStartScale, loopOutAnimParameters.easeTime).SetDelay(loopOutAnimParameters.delayAfterStop).SetEase(loopOutAnimParameters.easeType));
                s.SetLoops(loopCount, LoopType.Restart);
            }

            return s;
        }

        public Vector3 GetAnimationTypeWiseValue(AnimationValueType animationValueType, Vector3 customValue)
        {
            switch (startValueType)
            {
                case AnimationValueType.CustomValue:
                    return customValue;
                case AnimationValueType.StartValue:
                    return Vector3.zero;
                    //return GetStartTimeValue();
                case AnimationValueType.CurrentValue:
                    return GetCurrentValue();
            }
            return customValue;
        }

        public override void SaveStartTimeValue()
        {
            //_startTimeValue = _obj.transform.localScale;
        }


        //public Vector3 GetStartTimeValue()
        //{
        //    return _startTimeValue;
        //}

        public Vector3 GetCurrentValue()
        {
            return _obj.transform.localScale;
        }
    }

    [System.Serializable]
    class AlphaEffect : UIAnimation
    {
        bool _showStartCustomValue => startValueType == AnimationValueType.CustomValue;
        bool _showTargetCustomValue => targetValueType == AnimationValueType.CustomValue;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] AnimationValueType startValueType;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showStartCustomValue")] [AllowNesting] [SerializeField] private float startAlpha = 0f;
        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] AnimationValueType targetValueType;
        [EnableIf(EConditionOperator.And, "enableEffect", "_showTargetCustomValue")] [AllowNesting] [SerializeField] private float targetAlpha = 1f;

        [EnableIf("enableEffect")] [AllowNesting] [SerializeField] private CanvasGroup cg = null;

        float _currentStartAlpha;
        float _currentTargetAlpha;
        float _startTimeValue;

        public override void SetStartValuesBeforePlay()
        {
            if (!enableEffect)
            {
                return;
            }

            _currentStartAlpha = GetAnimationTypeWiseValue(startValueType, startAlpha);
            cg.alpha = _currentStartAlpha;
        }
         
        protected override Sequence PerformPlayAnim()
        {
            SetStartValuesBeforePlay();

            _currentStartAlpha = GetAnimationTypeWiseValue(startValueType, startAlpha);
            _currentTargetAlpha = GetAnimationTypeWiseValue(targetValueType, targetAlpha);


            Sequence s = DOTween.Sequence();
            s.SetDelay(delayToStart, false);

            if (loopCount == 0)
            {
                s.Append(cg.DOFade(_currentTargetAlpha, animationParameters.easeTime).SetEase(animationParameters.easeType));
            }
            else
            {
                s.Append(cg.DOFade(_currentTargetAlpha, loopInAnimParameters.easeTime).SetDelay(loopInAnimParameters.delayAfterStop).SetEase(loopInAnimParameters.easeType));
                s.Append(cg.DOFade(_currentStartAlpha, loopOutAnimParameters.easeTime).SetDelay(loopOutAnimParameters.delayAfterStop).SetEase(loopOutAnimParameters.easeType));
                s.SetLoops(loopCount, LoopType.Restart);
            }
            return s;
        }

        public float GetAnimationTypeWiseValue(AnimationValueType animationValueType, float customValue)
        {
            switch (startValueType)
            {
                case AnimationValueType.CustomValue:
                    return customValue;
                case AnimationValueType.StartValue:
                    return GetStartTimeValue();
                case AnimationValueType.CurrentValue:
                    return GetCurrentValue();
            }
            return customValue;
        }

        public override void SaveStartTimeValue()
        {
            _startTimeValue = cg.alpha;
        }

        public float GetStartTimeValue()
        {
            return _startTimeValue;
        }

        public float GetCurrentValue()
        {
            return cg.alpha;
        }        
    }
}
