using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("---Player Stuff---")]
    public GameObject player;
    public playerControlle playerScript;

    [Header("---UI---")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;

    [Header("---Game Goals---")]
    public int enemiesAlive;

    public bool isPaused;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerControlle>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
            {
                pasueState();
            }
            else
            {
                unpauseState();
            }
        }
    }

    public void pasueState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unpauseState()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        activeMenu.SetActive(false);
        activeMenu = null;
    }

    public void updateGameGoal(int amt)
    {
        enemiesAlive += amt;

        if (enemiesAlive <= 0)
        {
            pasueState();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
        }
    }
}
