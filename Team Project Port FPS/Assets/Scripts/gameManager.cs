using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("---Player Stuff---")]
    public GameObject player;
    public playerController playerScript;
    public GameObject playerSpawnPos;

    [Header("---UI---")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject shopMenu;
    public GameObject checkPointMsg;
    public GameObject shopMsg;
    public GameObject playerHitFlash;
    public Image playerHPBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI pointsTotalText;

    [Header("---Pickup References---")]
    public Transform pickupPos;
    public GameObject gunPistol;
    public GameObject gunSmg;
    public GameObject gunShotgun;
    public GameObject gunAssaultFull;
    public GameObject gunAssaultSemi;
    public GameObject gunSniper;
    public GameObject gunRocketLauncher;
    public GameObject laserPistol;
    public GameObject laserSmg;
    public GameObject laserShotgun;
    public GameObject laserAssaultFull;
    public GameObject laserAssaultSemi;
    public GameObject laserSniper;
    public GameObject laserRocketLauncher;

    [Header("---Game Goals---")]
    public int enemiesAlive;
    public int pointsTotal;

    public bool isPaused;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
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

    public void updateGameGoal(int amt, int points)
    {
        enemiesAlive += amt;
        enemiesRemainingText.text = enemiesAlive.ToString("F0");

        pointsTotal += points;
        pointsTotalText.text = pointsTotal.ToString("F0");

        if (enemiesAlive <= 0)
        {
            pasueState();
            activeMenu = winMenu;
            activeMenu.SetActive(true);
        }
    }

    public void playerDead()
    {
        pasueState();
        activeMenu = loseMenu;
        activeMenu.SetActive(true);
    }
    public IEnumerator playerHit()
    {
        playerHitFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerHitFlash.SetActive(false);
    }
}
