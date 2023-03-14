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

    public void gunPistol()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunPistol, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void gunSmg()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunSmg, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void gunAssaultFull()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunAssaultFull, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void gunAssaultSemi()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunAssaultSemi, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void gunShotgun()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunShotgun, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void gunSniper()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunSniper, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void gunRocketLauncher()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.gunRocketLauncher, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void laserPistol()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserPistol, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void laserSmg()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserSmg, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void laserAssaultFull()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserAssaultFull, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void laserAssaultSemi()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserAssaultSemi, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void laserShotgun()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserShotgun, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void laserSniper()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserSniper, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }

    public void laserRocketLauncher()
    {
        if (gameManager.instance.pointsTotal >= 100)
        {
            Instantiate(gameManager.instance.laserRocketLauncher, gameManager.instance.pickupPos.position, gameManager.instance.pickupPos.rotation);
        }
    }
}
