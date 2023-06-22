using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.UIPackage.Animtion
{
    [System.Serializable]
    public class ParllelAnimHandller
    {
        public List<UIOpenCloseAnim> openCloseAnims;

        public float GetPlayOpenTime()
        {
            float animationMaxTime = 0f;
            float currentAnimationTime = 0f;

            for (int i = 0; i < openCloseAnims.Count; i++)
            {
                currentAnimationTime = openCloseAnims[i].GetOpenPlayTime();
                if (currentAnimationTime > animationMaxTime)
                    animationMaxTime = currentAnimationTime;
            }
            return animationMaxTime;
        }

        public float PlayOpenAnims()
        {
            for (int i = 0; i < openCloseAnims.Count; i++)
            {
                openCloseAnims[i].PlayOpenMotion();
            }

            return GetPlayOpenTime();
        }

        public void SetValuesBeforePlayForOpen()
        {
            for (int i = 0; i < openCloseAnims.Count; i++)
            {
                openCloseAnims[i].SetValuesBeforePlayForOpen();
            }
        }

        public float GetPlayCloseTime()
        {
            float animationMaxTime = 0f;
            float currentAnimationTime = 0f;

            for (int i = 0; i < openCloseAnims.Count; i++)
            {
                currentAnimationTime = openCloseAnims[i].GetClosePlayTime();
                if (currentAnimationTime > animationMaxTime)
                    animationMaxTime = currentAnimationTime;
            }
            return animationMaxTime;
        }

        public float PlayCloseAnims()
        {
            for (int i = 0; i < openCloseAnims.Count; i++)
            {
                openCloseAnims[i].PlayCloseMotion();
            }

            return GetPlayCloseTime();
        }


        public void SetValuesBeforePlayForClose()
        {
            for (int i = 0; i < openCloseAnims.Count; i++)
            {
                openCloseAnims[i].SetValuesBeforePlayForClose();
            }
        }
    }
}