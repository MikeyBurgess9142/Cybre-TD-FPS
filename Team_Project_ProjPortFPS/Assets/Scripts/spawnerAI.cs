using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Transform spawnPos;

    [Header("----- Spawn Stats -----")]
    [SerializeField] int spawnDelay;
    [SerializeField] int spawnAmount;

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
