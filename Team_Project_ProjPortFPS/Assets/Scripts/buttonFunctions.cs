using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.unpauseState();
        gameManager.instance.isPaused = !gameManager.instance.isPaused;
    }

    public void restart()
    {
        gameManager.instance.unpauseState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void respawnPlayer()
    {
        gameManager.instance.unpauseState();
        gameManager.instance.playerScript.respawnPlayer();
    }

    //Shop Menu Buttons

    public void exitShop()
    {
        gameManager.instance.unpauseState();
    }

    public void gunPistol()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunPistol, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button1.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void gunSmg()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunSmg, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button2.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void gunAssaultFull()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunAssaultFull, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button3.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void gunAssaultSemi()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunAssaultSemi, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button4.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void gunShotgun()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunShotgun, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button5.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void gunSniper()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunSniper, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button6.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void gunRocketLauncher()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunRocketLauncher, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button7.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void laserPistol()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserPistol, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button8.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void laserSmg()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserSmg, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button9.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void laserAssaultFull()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserAssaultFull, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button10.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void laserAssaultSemi()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserAssaultSemi, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button11.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void laserShotgun()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserShotgun, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button12.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void laserSniper()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserSniper, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button13.enabled = false;
            gameManager.instance.unpauseState();
        }
    }

    public void laserRocketLauncher()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserRocketLauncher, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
            gameManager.instance.button14.enabled = false;
            gameManager.instance.unpauseState();
        }
    }
}
