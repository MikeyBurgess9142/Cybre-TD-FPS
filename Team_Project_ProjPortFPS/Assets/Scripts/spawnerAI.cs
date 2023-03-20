using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerAI : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] Transform spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void createObject()
    {
        GameObject objectClone = Instantiate(objectToSpawn, spawnPos.position, objectToSpawn.transform.rotation);
    }
}
