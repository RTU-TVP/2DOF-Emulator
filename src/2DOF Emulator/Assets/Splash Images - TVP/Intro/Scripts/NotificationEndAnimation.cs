using UnityEngine;
using UnityEngine.Events;

namespace SplashImages.Intro
{
    public class NotificationEndAnimation : MonoBehaviour
    {
        public event UnityAction OnEndAnimation;

        public void EndAnimation()
        {
            OnEndAnimation?.Invoke();
        }
    }
}