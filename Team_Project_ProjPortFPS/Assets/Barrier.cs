using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    // Start is called before the first frame update
    private int hp;
    GameObject currentBarrier;

    public int weakBarrierHp;
    public int midBarrierHp;
    public int strongBarrierHp;
    public GameObject barriercollider;
    public GameObject defaultBarrier;
    public GameObject weakBarrier;
    public GameObject midBarrier;
    public GameObject strongBarrier;

   
    public bool barrierActive = false;
    bool canUpgrade = false;
    bool upgradeMax = false;
   
 
    
    public void Start()
    {
        DefaultBarrier();

    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && canUpgrade == true )
        {
           

            if (gameManager.instance.pointsTotal >= 150 && currentBarrier.name == "MidBarrier(Clone)")
            {
                StrongUpgrade();
            }


            if (gameManager.instance.pointsTotal >= 100 && currentBarrier.name == "WeakBarrier(Clone)")
            {
                MidUpgrade();
            }

            if (gameManager.instance.pointsTotal >= 50 && currentBarrier.name == "DefaultBarrier(Clone)")
            {
                
                WeakUpgrade();
            }
        }

        if (currentBarrier == null)
        {

            DefaultBarrier();
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canUpgrade = true;
            if (upgradeMax)
            {
                gameManager.instance.maxUpgradeMsg.SetActive(true);
            }
            else
            {
                gameManager.instance.interactMessage.SetActive(true);
            }
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            canUpgrade = false;
            gameManager.instance.maxUpgradeMsg.SetActive(false);
            gameManager.instance.interactMessage.SetActive(false);
        }

    }

    public void DefaultBarrier()
    {
        barriercollider.SetActive(false);
        barrierActive = false;
        upgradeMax = false;
        currentBarrier = Instantiate(defaultBarrier, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
    public void WeakUpgrade()
    {
        
        gameManager.instance.pointsTotal -= 50;
        Destroy(currentBarrier);
        currentBarrier = Instantiate(weakBarrier, this.gameObject.transform.position, this.gameObject.transform.rotation);
        hp = weakBarrierHp;
        barriercollider.SetActive(true);
        barrierActive = true;
        
    }

    public void MidUpgrade()
    {
        gameManager.instance.pointsTotal -= 100;
        Destroy(currentBarrier);
        currentBarrier = Instantiate(midBarrier, this.gameObject.transform.position, this.gameObject.transform.rotation);
        hp = midBarrierHp;
        barrierActive = true;
        gameManager.instance.maxUpgradeMsg.SetActive(true);
        gameManager.instance.interactMessage.SetActive(false);

    }

    public void StrongUpgrade()
    {
        gameManager.instance.pointsTotal -= 150;
        upgradeMax = true;


        Destroy(currentBarrier);
        currentBarrier = Instantiate(strongBarrier, this.gameObject.transform.position, this.gameObject.transform.rotation);
        hp = strongBarrierHp;
        barrierActive = true;
       
    }

    public void TakeDmg(int dmg)
    {
        if (currentBarrier.name != "DefaultBarrier(Clone)")
        {
            hp -= dmg;

            if (hp <= 0)
            {
                StopAllCoroutines();
                Destroy(currentBarrier);
                gameManager.instance.maxUpgradeMsg.SetActive(false);
                gameManager.instance.interactMessage.SetActive(true);
                DefaultBarrier();
            }
        }
    }

    
}
