using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;

    [Header("----- Stats -----")]
    [SerializeField] Transform headPos;
    [Range(1, 500)][SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    [SerializeField] int pointValue;

    [Header("----- Gun Stats -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;
    [SerializeField]int playerInRangeDist;

    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistOrig;

    Vector3 playerDir;
    Vector3 shootDir;
    bool isShooting;
    bool playerInRange;
    float angleToPlayer;
    
    float speedOrig;
    Color origColor;

    void Start()
    {
        origColor = model.material.color;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;
        gameManager.instance.updateGameGoal(1,0,0,0);
        gameManager.instance.addEnemy(agent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
        if (other.CompareTag("Ally"))
        {
            gameManager.instance.allyScript.currEnemy = gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            anim.SetFloat("Speed", agent.velocity.normalized.magnitude);
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (agent.remainingDistance <= playerInRangeDist)
            {
                canSeePlayer();
            }
        }
    }
    bool canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position).normalized;
        shootDir = (gameManager.instance.playerScript.playerHitBox.position - headPos.position).normalized;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    facePlayer();
                }

                if (!isShooting && angleToPlayer <= shootAngle)
                {
                    StartCoroutine(shoot());
                }

                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    public void takeDmg(int dmg)
    {
        HP -= dmg;
        anim.SetTrigger("Damage");
        if (HP <= 0)
        {
            if (GetComponent<CapsuleCollider>() != null)
            {
                GetComponent<CapsuleCollider>().enabled = false;
            }
            agent.enabled = false;
            Destroy(gameObject);
            gameManager.instance.updateGameGoal(-1, 0, 0, pointValue);
        }
        else
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(flashDamage());
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = origColor;
    }

    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
    public void createBullet()
    {
        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        //bulletClone.GetComponent<Rigidbody>().velocity = (transform.forward + new Vector3(0, playerDir.y, 0)) * bulletSpeed;
        bulletClone.GetComponent<Rigidbody>().velocity = shootDir * bulletSpeed;
    }

    public void agentStop()
    {
        agent.enabled = false;
    }

    public void agentStart()
    {
        agent.enabled = true;
    }
}
