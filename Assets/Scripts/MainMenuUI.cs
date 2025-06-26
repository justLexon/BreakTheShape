using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void OnPlayButtonLoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnPlayButtonLoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
