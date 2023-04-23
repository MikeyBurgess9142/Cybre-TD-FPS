using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("---Player Stuff---")]
    public GameObject player;
    public playerController_Old playerScript;
    public PlayerController playerControllerScript;
    public GameObject playerSpawnPos;
    public allyAI allyScript;

    [Header("---UI---")]
    public GameObject activeMenu;
    public GameObject pauseMenu;
    public GameObject winMenu;
    public GameObject loseMenu;
    public GameObject shopMenu;
    public GameObject checkPointMsg;
    public GameObject shopMsg;
    public GameObject interactMessage;
    public GameObject maxUpgradeMsg;
    public GameObject playerHitFlash;
    public Image playerHPBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI bossesKilledText;
    public TextMeshProUGUI wavesCurrentText;
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
    public GameObject healthSmall;
    public GameObject healthMed;
    public GameObject healthLrg;

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
    public Button button15;
    public Button button16;
    public Button button17;

    [Header("----- Spawner Stats -----")]
    public List<spawnerAI> spawners;
    public List<spawnerAI> bossSpawners;
    public int bossWaveInterval; //if set to 3 boss's spawn on waves 3, 6, 9...
    public int spawnIntensity;
    public int intensityIncreaseAmt;
    public int waveDelay;

    [Header("---Game Goals---")]
    public int enemiesAlive;
    public int bossesAlive;
    public int pointsTotal;
    public bool isPaused;
    public bool waveStarted;
    public int numberOfWaves;
    public int waveNumber;
    public int wavesCurrent;
    public int bossesKilled;

    public List<NavMeshAgent> enemy;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController_Old>();
        playerControllerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
    }

    private void Start()
    {
        //StartCoroutine(startWave());
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

    public void updateGameGoal(int amt, int bamt, int bkamt, int points, bool fromShop = false)
    {
        enemiesAlive += amt;
        enemiesRemainingText.text = enemiesAlive.ToString("F0");

        bossesAlive += bamt;

        bossesKilled += bkamt;
        bossesKilledText.text = bossesKilled.ToString("F0");

        wavesCurrent = waveNumber;
        wavesCurrentText.text = wavesCurrent.ToString("F0");

        pointsTotal += points;
        pointsTotalText.text = pointsTotal.ToString("F0");

        if (enemiesAlive <= 0 && !fromShop)
        {
            Debug.Log("Wave Ended");
            StartCoroutine(startWave());
            wavesCurrent = waveNumber;
            wavesCurrentText.text = wavesCurrent.ToString("F0");
            if (spawnIntensity % bossWaveInterval == 0)
            {
                startBossWave();
            }
        }
        if (numberOfWaves < waveNumber && bossesAlive <= 0)
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

    public void addEnemy(NavMeshAgent enmy)
    {
        enemy.Add(enmy);
    }

    public IEnumerator startWave()
    {
        if (!waveStarted)
        {
            waveStarted = true;
            yield return new WaitForSeconds(waveDelay);
            waveNumber++;
            spawnIntensity += intensityIncreaseAmt;
            waveStarted = false;
            foreach (spawnerAI spawner in spawners)
            {
                Debug.Log("Spawner Activated");

               // StartCoroutine(spawner.spawnWave(spawnIntensity));

              // StartCoroutine(spawner.spawnWave(spawnIntensity));

            }
        }
    }
    public void startBossWave()
    {
        foreach (spawnerAI spawner in bossSpawners)
        {
            Debug.Log("Spawner Activated");
            if (bossSpawners.Count > 0)
            {
               // StartCoroutine(spawner.spawnWave(spawnIntensity / bossWaveInterval));
            }
        }
    }
}
