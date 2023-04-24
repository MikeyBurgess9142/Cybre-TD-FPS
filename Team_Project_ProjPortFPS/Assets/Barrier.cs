using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    TextMeshProUGUI pointsTotalText;
    int upgradePointcost = 50;
   
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
                pointsTotalText = GameObject.Find("CostText").GetComponent<TextMeshProUGUI>();
                pointsTotalText.text = string.Format("{0} points.", upgradePointcost);
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
        upgradePointcost = 50;

        barriercollider.SetActive(false);
        barrierActive = false;
        upgradeMax = false;
        currentBarrier = Instantiate(defaultBarrier, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
    public void WeakUpgrade()
    {
        
        
        gameManager.instance.pointsTotal -= upgradePointcost;
        gameManager.instance.pointsTotalText.text = gameManager.instance.pointsTotal.ToString("F0");
        Destroy(currentBarrier);
        currentBarrier = Instantiate(weakBarrier, this.gameObject.transform.position, this.gameObject.transform.rotation);
        hp = weakBarrierHp;
        barriercollider.SetActive(true);
        barrierActive = true;
        upgradePointcost += 50;
        pointsTotalText.text = string.Format("{0} points.", upgradePointcost);
    }

    public void MidUpgrade()
    {
        gameManager.instance.pointsTotal -= upgradePointcost;
        gameManager.instance.pointsTotalText.text = gameManager.instance.pointsTotal.ToString("F0");
        Destroy(currentBarrier);
        currentBarrier = Instantiate(midBarrier, this.gameObject.transform.position, this.gameObject.transform.rotation);
        hp = midBarrierHp;
        barrierActive = true;
       
        upgradePointcost += 50;
        pointsTotalText.text = string.Format("{0} points.", upgradePointcost);
    }

    public void StrongUpgrade()
    {
        gameManager.instance.pointsTotal -= upgradePointcost;
        gameManager.instance.pointsTotalText.text = gameManager.instance.pointsTotal.ToString("F0");
        upgradeMax = true;


        Destroy(currentBarrier);
        currentBarrier = Instantiate(strongBarrier, this.gameObject.transform.position, this.gameObject.transform.rotation);
        hp = strongBarrierHp;
        barrierActive = true;
        gameManager.instance.maxUpgradeMsg.SetActive(true);

        gameManager.instance.interactMessage.SetActive(false);
        upgradePointcost = 50;
        pointsTotalText.text = string.Format("{0} points.", upgradePointcost);
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
