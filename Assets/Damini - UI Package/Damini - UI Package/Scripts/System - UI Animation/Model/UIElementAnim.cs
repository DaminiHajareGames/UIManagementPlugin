using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Base.UIPackage.Animtion
{
    public class UIElementAnim : MonoBehaviour
    {
        [SerializeField] UIAnimationBundle motion;

        private void Awake()
        {
            motion.SetProperties(this);
        }
        
        [ContextMenu("Play")]
        public float Play()
        {
            return motion.Play();
        }

        public void StopLoopModeAnimation(AnimationType animationType)
        {
            motion.StopLoopModeAnimation(animationType);
        }
    }
}