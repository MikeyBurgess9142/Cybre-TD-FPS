using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerAI : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Transform spawnPos;

    [SerializeField] int spawnDelay;

    int spawnAmount;

    public IEnumerator spawnWave()
    {
        for (int i = 0; i < spawnAmount; i++) 
        {
            yield return new WaitForSeconds(spawnDelay);
            createObject();
        }
    }
    public void createObject()
    {
        GameObject objectClone = Instantiate(objectToSpawn, spawnPos.position, objectToSpawn.transform.rotation);
    }
}
