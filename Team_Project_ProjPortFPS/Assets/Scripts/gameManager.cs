using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;
    private Vector3 playerPos;
    private Quaternion playerRot;
    private Vector3 currentplayerPos;
    private Quaternion currentplayerRot;

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
    public GameObject turretMessage;
    public GameObject maxUpgradeMsg;
    public GameObject playerHitFlash;
    public Image playerHPBar;
    public TextMeshProUGUI enemiesRemainingText;
    public TextMeshProUGUI bossesKilledText;
    public TextMeshProUGUI bossesAliveText;
    public TextMeshProUGUI civilliansRescuedText;
    public TextMeshProUGUI civilliansTotalText;
    public TextMeshProUGUI pointsTotalText;
 

    [Header("---Pickup References---")]
    public Transform pickupPos;
    public GameObject gunPistol;
    public GameObject personalTurret;
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
    public Button button18;
    [Header("----- Spawner Stats -----")]
    public List<spawnerAI> spawners;
    public List<spawnerAI> bossSpawners;
    public int bossWaveInterval; //if set to 3 boss's spawn on waves 3, 6, 9...
    public int spawnIntensity;
    public int intensityIncreaseAmt;
    public int waveDelay;

    [Header("---Game Goals---")]
    public int enemiesAlive;
    public int bossesTotal;
    public int pointsTotal;
    public bool isPaused;
    public bool waveStarted;
    public int numberOfWaves;
    public int civillians;
    public int civilliansRescued;
    public int bossesKilled;
    public int friendsSaved;

    public List<NavMeshAgent> enemy;

    void Awake()
    {
        PlayerPrefs.DeleteAll();
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController_Old>();
        playerControllerScript = player.GetComponent<PlayerController>();
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        personalTurret = GameObject.Find("TurretPos");
    }

    private void Start()
    {
        pointsTotalText.text = pointsTotal.ToString("F0");
        //StartCoroutine(startWave());
    }

    void Update()
    {
        //currentplayerPos = playerScript.gameObject.transform.position;
        //currentplayerRot = playerScript.gameObject.transform.rotation;
        //currentplayerPos = playerControllerScript.gameObject.transform.position;
        //currentplayerRot = playerControllerScript.gameObject.transform.rotation;
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            isPaused = !isPaused;
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);

            if (isPaused)
            {
                pauseState();
            }
            else
            {
                unpauseState();
            }
        }
    }

    public  void LoadGame()
    {   
        string sceneName = PlayerPrefs.GetString("CurrentScene");
        SceneManager.LoadScene(sceneName);
        unpauseState();
    }
    public void SpawnPlayer()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        player.transform.position = playerPos;
        player.transform.rotation = playerRot;
    }
    public  void SaveGame()
    {    
        PlayerPrefs.SetInt("PlayerHealth", playerScript.HP);
        PlayerPrefs.SetInt("civilliansRescued", civilliansRescued);
      
        PlayerPrefs.SetInt("pointsTotal", pointsTotal);
        //PlayerPrefs.SetString("CurrentGun", currentGun);
        PlayerPrefs.SetInt("bossesKilled", bossesKilled);
     
        PlayerPrefs.SetString("CurrentScene", SceneManager.GetActiveScene().name);
        //saves player position
        PlayerPrefs.SetFloat("PlayerPosX", currentplayerPos.x);
        PlayerPrefs.SetFloat("PlayerPosY", currentplayerPos.y);
        PlayerPrefs.SetFloat("PlayerPosZ", currentplayerPos.z);
        //saves player rotation
        PlayerPrefs.SetFloat("PlayerRotX", currentplayerRot.x);
        PlayerPrefs.SetFloat("PlayerRotY", currentplayerRot.y);
        PlayerPrefs.SetFloat("PlayerRotZ", currentplayerRot.z);
        PlayerPrefs.SetFloat("PlayerRotW", currentplayerRot.w);
        
        PlayerPrefs.Save();
    }

    public void pauseState()
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
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
       
        activeMenu = null;
        
    }

    public void updateGameGoal(int enmsAlive, int bkamt, int bamt, int cramt, int camt, int points, bool fromShop = false)
    {
        enemiesAlive = enmsAlive;

        bossesTotal += bamt;
        bossesAliveText.text = bossesTotal.ToString("F0");

        bossesKilled += bkamt;
        bossesKilledText.text = bossesKilled.ToString("F0");

        civillians += camt;
        civilliansTotalText.text = civillians.ToString("F0");

        civilliansRescued += cramt;
        civilliansRescuedText.text = civilliansRescued.ToString("F0");


       pointsTotal += points;
        pointsTotalText.text = pointsTotal.ToString("F0");

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0))
        {
            if (bossesKilled == bossesTotal && civilliansRescued == civillians )
            {
                pauseState();
                SceneManager.LoadScene(2);
                unpauseState();
            }
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0))
        {
            if (bossesKilled == bossesTotal && civilliansRescued == civillians)
            {
                pauseState();
                SceneManager.LoadScene(3);
                unpauseState();
            }
        }

        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(0))
        {
            if (bossesKilled == bossesTotal && civilliansRescued == civillians)
            {
                pauseState();
                activeMenu = winMenu;
                activeMenu.SetActive(true);
            }
        }
    }

    public void playerDead()
    {
        pauseState();

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
            //waveNumber++;
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
