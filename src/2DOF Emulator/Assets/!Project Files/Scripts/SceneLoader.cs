using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private AsyncOperation _sceneAsync;

    private void Start()
    {
        _sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        if (_sceneAsync != null)
        {
            _sceneAsync.allowSceneActivation = false;
        }
        else
        {
            Debug.LogError($"SceneLoader: Scene '{sceneName}' not found.");
        }
    }

    public void LoadScene()
    {
        _sceneAsync.allowSceneActivation = true;
    }
}