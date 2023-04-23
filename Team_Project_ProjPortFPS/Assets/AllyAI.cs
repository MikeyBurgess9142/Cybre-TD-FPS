using UnityEngine;
using UnityEngine.AI;

public class AllyAI : MonoBehaviour
{
    public Transform destination;
    public float walkSpeed = 1f;
    public float enemyDetectionRange = 10f;
    public LayerMask enemyLayer;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool isCowering = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = walkSpeed;
        navMeshAgent.destination = destination.position;
    }

    private void Update()
    {
        if (DetectEnemies())
        {
            Cower();
        }
        else
        {
            StopCowering();
            navMeshAgent.destination = destination.position;
        }
    }

    private bool DetectEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, enemyDetectionRange, enemyLayer);
        return enemiesInRange.Length > 0;
    }

    private void Cower()
    {
        if (!isCowering)
        {
            animator.SetBool("Cower", true);
            isCowering = true;
            navMeshAgent.isStopped = true;
        }
    }

    private void StopCowering()
    {
        if (isCowering)
        {
            animator.SetBool("Cower", false);
            isCowering = false;
            navMeshAgent.isStopped = false;
        }
    }
}