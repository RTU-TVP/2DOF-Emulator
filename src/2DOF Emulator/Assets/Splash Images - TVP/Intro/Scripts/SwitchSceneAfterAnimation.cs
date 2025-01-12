using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SplashImages.Intro
{
    public class SwitchSceneAfterAnimation : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private NotificationEndAnimation notificationEndAnimation;

        private AsyncOperation _asyncOperation;

        public void Start()
        {
            _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            _asyncOperation.allowSceneActivation = false;

            notificationEndAnimation.OnEndAnimation += SwitchScene;
        }

        private void SwitchScene()
        {
            notificationEndAnimation.OnEndAnimation -= SwitchScene;
            _asyncOperation.allowSceneActivation = true;
        }
    }
}