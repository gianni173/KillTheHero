using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] 
    private float _sceneLoadDelay = 1f;

    [SerializeField] 
    private string _sceneToLoadName = "SceneName";

    [SerializeField]
    private bool _loadSceneAtStart;

    private void Start()
    {
        if (!_loadSceneAtStart)
        {
            return;
        }
        StartCoroutine(LoadSceneWithDelay(_sceneLoadDelay, _sceneToLoadName));
    }

    public static IEnumerator LoadSceneWithDelay(float delay, string sceneName)
    {
        Debug.Log("Loading time startup: " + delay + " seconds");
        yield return new WaitForSecondsRealtime(delay);
        LoadScene(sceneName);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}