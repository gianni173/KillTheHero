using UnityEngine;

public class DebuggerInput : MonoBehaviour
{
    [SerializeField] private string _mainScene;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.LoadScene(_mainScene);
        }
    }
}