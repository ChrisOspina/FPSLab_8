using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
        GameManager.gm.gameState = GameState.Level1Playing;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
