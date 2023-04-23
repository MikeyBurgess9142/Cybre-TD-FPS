using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerAI : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject objectToSpawn;

    [Header("----- Spawn Stats -----")]

    [SerializeField] int spawnAmount;

    private void Start()
    {
        spawnWave(spawnAmount);
    }
   
    public void spawnWave(int spawnAmount)
    {
        Debug.Log("Wave Spawning");
        for (int i = 0; i < spawnAmount ; i++) 
        {
           
            createObject();
        }
    }
    public void createObject()
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        Debug.Log("Object Spawned");
    }
}
