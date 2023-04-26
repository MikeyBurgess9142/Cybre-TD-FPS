using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class allyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;

    [Header("----- Stats -----")]
    [SerializeField] Transform headPos;
    [SerializeField] Transform enemyHitBox;
    [Range(1, 500)] [SerializeField] int HP;
    [SerializeField] int enemyFaceSpeed;
    [SerializeField] int viewAngle;
    [SerializeField] int shootAngle;
    [SerializeField] int waitTime;
    [SerializeField] int roamDist;

    [Header("----- Gun Stats -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] int bulletSpeed;
    [SerializeField] float shootRate;
    [SerializeField] int shootDamage;

    Vector3 startingPos;
    bool destinationChosen;
    float stoppingDistOrig;

    Vector3 enemyDir;
    Vector3 shootDir;
    bool enemyInRange;
    public GameObject currEnemy;
    bool isShooting;
    float angleToEnemy;
    float speedOrig;
    Color origColor;

    void Start()
    {
        origColor = model.material.color;
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        speedOrig = agent.speed;

    }

    void Update()
    {
        if (agent.isActiveAndEnabled)
        {

            if (enemyInRange)
            {
                if (!canSeeEnemy())
                {
                    StartCoroutine(roam());
                }
            }
            else if (agent.destination != currEnemy.transform.position)
            {
                StartCoroutine(roam());
            }
        }
    }

    IEnumerator roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05f)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            agent.speed = speedOrig;
            yield return new WaitForSeconds(waitTime);
            destinationChosen = false;

            if (agent.isActiveAndEnabled)
            {
                Vector3 randDir = Random.insideUnitSphere * roamDist;
                randDir += startingPos;

                NavMeshHit hit;
                NavMesh.SamplePosition(randDir, out hit, roamDist, NavMesh.AllAreas);

                agent.SetDestination(hit.position);
            }
        }
    }

    bool canSeeEnemy()
    {
        
        enemyDir = (currEnemy.transform.position - headPos.position).normalized;
        shootDir = (currEnemy.transform.position - headPos.position).normalized;
        angleToEnemy = Vector3.Angle(new Vector3(enemyDir.x, 0, enemyDir.z), transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(headPos.position, enemyDir, out hit))
        {
            if (hit.collider.CompareTag("Enemy") && angleToEnemy <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(currEnemy.transform.position);

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    faceEnemy();
                }

                if (!isShooting && angleToEnemy <= shootAngle)
                {
                    StartCoroutine(shoot());
                }

                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }
    public void targetEnemy()
    {

    }

    public void takeDmg(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            agent.enabled = false;
            GameObject.Destroy(gameObject);
        }
        else
        {
            agent.SetDestination(currEnemy.transform.position);
            StartCoroutine(flashDamage());
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = origColor;
    }

    void faceEnemy()
    {
        enemyDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(enemyDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * enemyFaceSpeed);
    }

    IEnumerator shoot()
    {
        isShooting = true;
        GameObject bulletClone = Instantiate(bullet, shootPos.position, bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = shootDir * bulletSpeed;
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyInRange = true;
        }   
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemyInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    public void agentStop()
    {
        agent.enabled = false;
    }

    public void agentStart()
    {
        agent.enabled = true;
    }
    //public void OnParticleCollision(GameObject other)
    //{
    //        
    //}
}
