using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private float mainSceneLoadTime = 5f;

    void Start()
    {
        StartCoroutine(LoadTimeStartup());
    }

    IEnumerator LoadTimeStartup()
    {
        Debug.Log("Loading time startup: " + mainSceneLoadTime + " seconds");
        yield return new WaitForSecondsRealtime(mainSceneLoadTime);
        Debug.Log("Loading scene");
        LoadScene("MainScene");
    }

    public static void LoadScene(string sceneName)
    {
        Debug.Log("Loading " + sceneName);
        SceneManager.LoadSceneAsync(sceneName);
    }
}