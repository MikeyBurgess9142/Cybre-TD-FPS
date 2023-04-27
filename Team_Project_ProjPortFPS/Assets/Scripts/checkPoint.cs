using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerSpawnPos.transform.position = transform.position;
            gameManager.instance.SaveGame();
            StartCoroutine(popCheckPointMsg());
        }
    }

   

    IEnumerator popCheckPointMsg()
    {
        gameManager.instance.checkPointMsg.SetActive(true);
        yield return new WaitForSeconds(1);
        gameManager.instance.checkPointMsg.SetActive(false);
    }
}
