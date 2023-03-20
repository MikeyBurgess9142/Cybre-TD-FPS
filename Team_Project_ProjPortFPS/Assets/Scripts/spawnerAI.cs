using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject objectToSpawn;

    [Header("----- Spawn Stats -----")]
    [SerializeField] int spawnDelay;
    [SerializeField] int spawnAmount;

    public IEnumerator spawnWave()
    {
        Debug.Log("Wave Spawning");
        for (int i = 0; i < spawnAmount; i++) 
        {
            yield return new WaitForSeconds(spawnDelay);
            createObject();
        }
    }
    public void createObject()
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        Debug.Log("Object Spawned");
    }
}
