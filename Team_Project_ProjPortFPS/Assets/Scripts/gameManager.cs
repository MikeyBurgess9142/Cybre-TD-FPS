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
    public playerController_Old playerScript;
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

    [Header("---Shop Button References---")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Button button7;
    public Button button8;
    public Button button9;
    public Button button10;
    public Button button11;
    public Button button12;
    public Button button13;
    public Button button14;

    [Header("---Game Goals---")]
    public int enemiesAlive;
    public int pointsTotal;

    public bool isPaused;
    public GameObject[] enemy;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        playerScript = player.GetComponent<playerController_Old>();
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
