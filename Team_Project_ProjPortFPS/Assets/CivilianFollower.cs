using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilianFollower : MonoBehaviour
{
    private GameObject player;
    public Transform targetLocation;
    private GameObject targetCollider;
    public float stopDistance = 1.0f;
    

    private NavMeshAgent agent;
    private Animator animator;
    private bool isFollowing = false;
    private bool hasReachedTarget = false;
    private bool targetAreaReached = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = gameManager.instance.player;
        targetCollider = GameObject.Find("SpawnRoom");
        gameManager.instance.updateGameGoal(0, 0, 0, 1, 0, 0);
    }

    private void Update()
    {
        if (isFollowing && !hasReachedTarget && !targetAreaReached)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (targetAreaReached && !hasReachedTarget)
        {
            agent.SetDestination(targetLocation.position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Debug.Log("boom");
                isFollowing = false;
                hasReachedTarget = true;
                agent.isStopped = true;
                animator.SetBool("IsSadIdle", false);
                animator.SetBool("IsRunning", false);
                animator.SetTrigger("ReachedTarget");
                gameManager.instance.updateGameGoal(0, 0, 0, 0, 1, 0);
            }
        }
       
            UpdateAnimations();
        
      
    }

    private void UpdateAnimations()
    {
        if (!hasReachedTarget)
        {
            
            if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
            {
                animator.SetBool("IsRunning", true);
                animator.SetBool("IsSadIdle", false);
            }
            else
            {
                animator.SetBool("IsRunning", false);
                animator.SetBool("IsSadIdle", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isFollowing = true;
            agent.isStopped = false;
        }
        else if (other.gameObject == targetCollider)
        {
            targetAreaReached = true;
        }
    }
}