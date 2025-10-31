using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    // Funzioni da mettere sui bottoni dei menu

    public void PlayGame()
    {
        SceneLoader.LoadScene("GameplayScene");
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("StartupScene");
    }

    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}