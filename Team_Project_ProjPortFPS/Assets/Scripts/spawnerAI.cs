using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject objectToSpawn;

    [Header("----- Spawn Stats -----")]
    [SerializeField] float spawnDelay;
    [SerializeField] float spawnAmount;

    public IEnumerator spawnWave(int spawnIntensity)
    {
        Debug.Log("Wave Spawning");
        for (int i = 0; i < spawnAmount * spawnIntensity; i++) 
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
