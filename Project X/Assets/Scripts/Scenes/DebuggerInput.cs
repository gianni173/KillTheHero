using UnityEngine;
using UnityEngine.InputSystem;

public class DebuggerInput : MonoBehaviour
{
    [SerializeField] private string _scene;
    [SerializeField] private KeyCode _button = KeyCode.Escape;

    void Update()
    {
        if (Input.GetKeyDown(_button))
        {
            SceneLoader.LoadScene(_scene);
        }
    }
}