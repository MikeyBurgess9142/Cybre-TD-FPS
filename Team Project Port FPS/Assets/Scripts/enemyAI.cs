using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("---Components---")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("---Enemy Stats---")]
    [SerializeField] Transform headPos;
    [Range(5, 500)] [SerializeField] int HP;
    [SerializeField] int roamDist;
    [SerializeField] int sightAngle;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int waitTime;

    [Header("---Gun Stats---")]
    [Range(0, 10)] [SerializeField] float shtRate;
    [Range(10, 500)] [SerializeField] int shtDist;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpd;
    [SerializeField] Transform shootPos;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    bool destinationChosen;
    float stoppingDistOrig;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (!isShooting)
        {
            StartCoroutine(shoot());
        }
    }

    bool canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - headPos.position);
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);

        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if(agent.remainingDistance < agent.stoppingDistance)
                {
                    
                }

                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }
        return false;
    }

    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * playerFaceSpeed);
    }
    IEnumerator shoot()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(bullet,shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpd;
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
