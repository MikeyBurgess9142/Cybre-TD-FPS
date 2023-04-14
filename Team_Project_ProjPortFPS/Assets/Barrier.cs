using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp;
    

    public void TakeDmg(int dmg)
    {
        hp -= dmg;
       
        if (hp <= 0)
        {
            StopAllCoroutines();
            Destroy(this.gameObject);
        }
    }

    
}
