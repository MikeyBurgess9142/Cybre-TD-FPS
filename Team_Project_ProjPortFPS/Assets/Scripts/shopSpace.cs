using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopSpace : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(popShopMsg());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shop();
        }
    }

    public void shop()
    {
        if (Input.GetButtonDown("Shop") && gameManager.instance.activeMenu == null)
        {
            gameManager.instance.pauseState();
            gameManager.instance.activeMenu = gameManager.instance.shopMenu;
            gameManager.instance.activeMenu.SetActive(true);
        }
        
    }

    IEnumerator popShopMsg()
    {
        gameManager.instance.shopMsg.SetActive(true);
        yield return new WaitForSeconds(3);
        gameManager.instance.shopMsg.SetActive(false);
    }

}
