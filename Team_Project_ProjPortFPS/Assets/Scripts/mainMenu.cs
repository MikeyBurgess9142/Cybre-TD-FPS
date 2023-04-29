using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public void LevelOne()
    {
        SceneManager.LoadScene(1);
        gameManager.instance.unpauseState();
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene(2);
        gameManager.instance.unpauseState();
    }

    public void LevelThree()
    {
        SceneManager.LoadScene(3);
        gameManager.instance.unpauseState();
    }

    public void LoadGame()
    {


        string sceneName = PlayerPrefs.GetString("CurrentScene");
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
