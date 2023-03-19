using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int dmg;
    [Range(1,5)][SerializeField] int timer;
    bool hit;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hit)
        {
            hit = true;
            gameManager.instance.playerScript.takeDmg(dmg);
        }
    }
}
