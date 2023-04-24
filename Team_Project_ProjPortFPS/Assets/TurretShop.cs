using UnityEngine;
using UnityEngine.UI;

public class TurretShop : MonoBehaviour
{
    public GameObject turret; // the prefab for the turret
    public GameObject turretDefault;
    public int turretCost = 100; // the cost of the turret
    

    private bool promptActive = false; // whether the prompt is currently active

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !promptActive)
        {
            gameManager.instance.turretMessage.SetActive(true); 
           

            promptActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && promptActive)
        {
            gameManager.instance.turretMessage.SetActive(false); // deactivate the prompt
            promptActive = false;
        }
    }

    private void Update()
    {
        if (promptActive && Input.GetKeyDown(KeyCode.F))
        {
            // check if the player has enough coins to buy the turret
            if (gameManager.instance.pointsTotal >= turretCost)
            {
                // subtract the cost from the player's coins
                gameManager.instance.pointsTotal -= turretCost;
                gameManager.instance.pointsTotalText.text = gameManager.instance.pointsTotal.ToString("F0");
                // instantiate the turret and activate it

                turret.SetActive(true);
                turretDefault.SetActive(false);
                // deactivate the prompt and disable the trigger
                gameManager.instance.turretMessage.gameObject.SetActive(false);
                promptActive = false;
                GetComponent<Collider>().enabled = false;
            }
            
        }
    }
}




