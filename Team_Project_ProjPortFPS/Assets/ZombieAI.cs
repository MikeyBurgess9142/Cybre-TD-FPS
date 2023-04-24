using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    private GameObject player;
    public int health = 100;
    public int damage = 10;
    public int pointsOnDeath;
    public float chaseDistance = 20.0f;
    private float attackDistance = 2.0f;
    public float stoppingDistance = 2f;
    public float rotationSpeed = 2f;
    public float runSpeed = 6f;
    public float attackRate = 1.0f;
    private float attackCooldown;
    private bool isInIdleState = false;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public LayerMask attackableLayers;
    private Collider[] potentialTargets;
    private GameObject currentAttackTarget;
    
    void Start()
    {
        player = gameManager.instance.player;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackCooldown = 0;
        navMeshAgent.speed = runSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
        attackDistance = navMeshAgent.stoppingDistance;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= chaseDistance)
        {
            LookAtTarget();

            if (distanceToPlayer > stoppingDistance)
            {
                navMeshAgent.SetDestination(player.transform.position);
                animator.SetBool("isRunning", true);
            }
            else
            {
                navMeshAgent.isStopped = true;
                animator.SetBool("isRunning", false);
            }

            potentialTargets = Physics.OverlapSphere(transform.position, attackDistance, attackableLayers);

            if (potentialTargets.Length > 0)
            {
                GameObject closestTarget = FindClosestTarget(potentialTargets);

                if (closestTarget != currentAttackTarget)
                {
                    currentAttackTarget = closestTarget;
                }

                if (attackCooldown <= 0 && !isInIdleState)
                {
                    AttackTarget(currentAttackTarget);
                    attackCooldown = attackRate;
                }
                else
                {
                    isInIdleState = true;
                    animator.SetBool("isRunning", false);
                    navMeshAgent.isStopped = true;
                }
            }
            else
            {
                navMeshAgent.isStopped = false;
                isInIdleState = false;
                animator.SetBool("isRunning", true);
            }
        }
        else
        {
            currentAttackTarget = null;
            isInIdleState = false;
            animator.SetBool("isRunning", false);
        }

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown <= 0)
            {
                isInIdleState = false;
            }
        }
        LookAtTarget();
    }

    public void Die()
    {
        // Add any death animations or effects here
        animator.SetTrigger("die");
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        navMeshAgent.enabled = false;
        this.enabled = false;
        Destroy(gameObject, 3);
        gameManager.instance.pointsTotal += pointsOnDeath;
        gameManager.instance.pointsTotalText.text = gameManager.instance.pointsTotal.ToString("F0");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }
    private int GetPriority(Collider target)
    {
        switch (target.tag)
        {
            case "Player":
                return Random.Range(1, 3);
            case "Ally":
                return Random.Range(1, 3);
            case "Barrier":
                return 3;
            default:
                return 0;
        }
    }
    private void LookAtTarget()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && currentAttackTarget != null)
        {
            Vector3 direction = (currentAttackTarget.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }


    }

    private GameObject FindClosestTarget(Collider[] targets)
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;
        int highestPriority = int.MinValue;

        foreach (Collider target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            int priority = GetPriority(target);

            if (priority > highestPriority || (priority == highestPriority && distance < closestDistance))
            {
                closestDistance = distance;
                closestTarget = target.gameObject;
                highestPriority = priority;
            }
        }

        return closestTarget;
    }

    void AttackTarget(GameObject target)
    {
        navMeshAgent.isStopped = true;
        animator.SetTrigger("attack");
        currentAttackTarget = target;
    }

    public void ApplyDamageToTarget()
    {
        if (currentAttackTarget != null)
        {
            
            if (currentAttackTarget.CompareTag("Barrier"))
            {
                Debug.Log("Attack");
                currentAttackTarget.transform.parent.gameObject.GetComponent<Barrier>().TakeDmg(damage);
            }
            if (currentAttackTarget.CompareTag("Player"))
            {
                gameManager.instance.player.GetComponent<playerController_Old>().takeDmg(damage);
            }
            if (currentAttackTarget.CompareTag("Ally"))
            {

            }
        }
    }
}