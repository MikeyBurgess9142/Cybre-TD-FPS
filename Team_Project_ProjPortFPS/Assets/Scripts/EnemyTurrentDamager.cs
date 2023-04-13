using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurrentDamager : MonoBehaviour
{
    public enemyAI ai;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CallTakeDamage(int dmg)
    {
        ai.takeDmg(dmg);
    }
}
