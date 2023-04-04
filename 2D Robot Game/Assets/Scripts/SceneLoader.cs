using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game Scene");
        Debug.Log("Load Game");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings Scene");
    }

    public void BackToStart()
    {
        SceneManager.LoadScene("Start Scene");
    }
}
