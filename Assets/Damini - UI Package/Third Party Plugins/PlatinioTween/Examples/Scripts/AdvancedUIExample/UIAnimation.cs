using UnityEngine;
using Platinio.TweenEngine;

namespace Platinio
{
    public abstract class UIAnimation : MonoBehaviour
    {
        [SerializeField] private string id = null;
        [SerializeField] protected Ease ease = Ease.EaseInBack;
        [SerializeField] protected float time = 0.0f;

        public float TIME { get { return time; } }

        public string ID { get { return id; } }

        public virtual BaseTween Play() {
            return null;
        }

        public abstract BaseTween PlayReverese();

        public void CallPlay()
        {
            Play();
        }
    }
}

