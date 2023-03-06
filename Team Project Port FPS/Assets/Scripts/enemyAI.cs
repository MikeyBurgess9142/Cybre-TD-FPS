using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("---Components---")]
    [SerializeField] Renderer model;

    [Header("---Enemy Stats---")]
    [Range(5, 500)] [SerializeField] int HP;

    [Header("---Gun Stats---")]
    [Range(0, 10)] [SerializeField] float shtRate;
    [Range(10, 500)] [SerializeField] int shtDist;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator shoot()
    {
        isShooting = true;
        yield return new WaitForSeconds(shtRate);
        isShooting = false;
    }

    public void takeDmg(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashMat());

        if (HP <=0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashMat()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }
}
