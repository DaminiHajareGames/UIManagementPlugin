using UnityEngine;
using Platinio.TweenEngine;
using System.Collections.Generic;
using System.Collections;

namespace Base.UIPackage.Animtion
{
    public class UIAnimator : MonoBehaviour
    {
        //private UIOpenCloseAnim[] animationArray = null;
        public SequencialAnimHandller sequencialAnimHandller;
        public SequencialAnimHandller sequencialCloseAnimHandller;

        [System.Serializable]
        public class SequencialAnimHandller
        {
            public List<ParllelAnimHandller> parallelAnimHandllers;
            MonoBehaviour _monoBehaviour;

            public void SetProperties(MonoBehaviour monoBehaviour)
            {
                _monoBehaviour = monoBehaviour;
            }

            public float GetPlayOpenTime()
            {
                float totalPlayTime = 0;

                for (int i = 0; i < parallelAnimHandllers.Count; i++)
                {
                    totalPlayTime += parallelAnimHandllers[i].GetPlayOpenTime();
                }

                return totalPlayTime;
            }

            public float PlayOpenAnims()
            {
                SetValuesBeforePlayForOpen();
                _monoBehaviour.StartCoroutine(PlayOpenAnimsCoroutine());
                return GetPlayOpenTime();
            }

            public void SetValuesBeforePlayForOpen()
            {
                for (int i = 0; i < parallelAnimHandllers.Count; i++)
                {
                    parallelAnimHandllers[i].SetValuesBeforePlayForOpen();
                }
            }

            IEnumerator PlayOpenAnimsCoroutine()
            {
                float openAnimTime;
                double startTime = Time.time;

                for (int i = 0; i < parallelAnimHandllers.Count; i++)
                {
                    openAnimTime = parallelAnimHandllers[i].PlayOpenAnims();
                    yield return new WaitForSeconds(openAnimTime);
                }
                yield return null;
            }

            public float GetPlayCloseTime()
            {
                float totalPlayTime = 0;

                for (int i = 0; i < parallelAnimHandllers.Count; i++)
                {
                    totalPlayTime += parallelAnimHandllers[i].GetPlayCloseTime();
                }

                return totalPlayTime;
            }

            public float PlayCloseAnims()
            {
                SetValuesBeforePlayForClose();
                _monoBehaviour.StartCoroutine(PlayCloseAnimsCoroutine());
                return GetPlayCloseTime();
            }

            public void SetValuesBeforePlayForClose()
            {
                for (int i = 0; i < parallelAnimHandllers.Count; i++)
                {
                    parallelAnimHandllers[i].SetValuesBeforePlayForClose();
                }
            }

            IEnumerator PlayCloseAnimsCoroutine()
            {
                float playTime;
                for (int i = 0; i < parallelAnimHandllers.Count; i++)
                {
                    playTime = parallelAnimHandllers[i].PlayCloseAnims();
                    yield return new WaitForSeconds(playTime);
                }
                yield return null;
            }
        }

        private void Awake()
        {
            sequencialAnimHandller.SetProperties(this);
            sequencialCloseAnimHandller.SetProperties(this);
        }

        public float GetAnimationOpenTime()
        {
            return sequencialAnimHandller.GetPlayOpenTime();
        }

        public float PlayOpenAnimations()
        {
            return sequencialAnimHandller.PlayOpenAnims();
        }

        public float GetAnimationCloseTime()
        {
            return sequencialCloseAnimHandller.GetPlayCloseTime();
        }

        public float PlayCloseAnimations()
        {
            return sequencialCloseAnimHandller.PlayCloseAnims();
        }
    }
}

