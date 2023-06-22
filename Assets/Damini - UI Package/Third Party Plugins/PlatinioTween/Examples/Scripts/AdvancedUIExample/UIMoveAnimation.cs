using Platinio.TweenEngine;
using UnityEngine;

namespace Platinio
{
    public class UIMoveAnimation : UIAnimation
    {
        [SerializeField] private Vector2 startPosition = Vector2.zero;
        [SerializeField] private Vector2 targetPosition = Vector2.zero;
        [SerializeField] private RectTransform canvas = null;


        public override BaseTween Play()
        {
            transform.position = startPosition;
            return GetComponent<RectTransform>().MoveUI(targetPosition , canvas , time).SetEase(ease);
        }

        public override BaseTween PlayReverese()
        {
            transform.position = targetPosition;
            return GetComponent<RectTransform>().MoveUI(startPosition, canvas, time).SetEase(ease);
        }
    }

}

