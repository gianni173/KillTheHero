using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwapSceneButtons : MonoBehaviour
{
    // Funzioni da mettere sui bottoni dei menu
    [SerializeField] 
    private Button _toGameplayButton;

    [SerializeField]
    private string _gameplaySceneName = "GameplayScene";

    [Space(24)]
    
    [SerializeField] 
    private Button _toMainMenuButton;

    [SerializeField]
    private string _mainMenuSceneName = "MainScene";

    [Space(24)]

    [SerializeField] 
    private Button _quitButton;

    private void Awake()
    {
        _toGameplayButton?.onClick.AddListener(PlayGame);
        _toMainMenuButton?.onClick.AddListener(ReturnMainMenu);
        _quitButton?.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        SceneLoader.LoadScene(_gameplaySceneName);
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}