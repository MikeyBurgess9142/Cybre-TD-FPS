using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalTurret : MonoBehaviour
{
    [Header("-----Turret-----")]
    public Transform firePoint;
    public float range = 10f;
    public float rotationSpeed = 5f;
    public float fireRate = 0.2f; // changed from 1f to 0.2f
    private float nextFireTime = 0f;
    public Transform playerTransform;
    public float followSpeed = 1f;
    public float followDistance = 5f;
    public float moveDistance = 1f;
    public float moveSpeed = 1f;
    public float maxRotationAngle;
    private Vector3 startPosition;
    private float movementOffset = 0f;
    [Header("-----Bullet-----")]
    public GameObject bulletPrefab;
    public int bulletDmg = 1;

    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        RemoveDeadEnemies();
        GameObject target = FindClosestEnemy();

        if (target != null)
        {
            RotateTowardsTarget(target);
            Shoot(target);
        }
        // Calculate the movement offset using a smooth motion
        movementOffset = Mathf.PingPong(Time.time * moveSpeed, moveDistance);

        // Calculate the target position based on the player's position and the movement offset
        Vector3 targetPosition = playerTransform.position + new Vector3(0f, followDistance + movementOffset, 0f);

        // Smoothly move the turret towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void RemoveDeadEnemies()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);
    }

    private GameObject FindClosestEnemy()
    {
        GameObject closestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy.GetComponent<ZombieAI>().health > 0)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    private void RotateTowardsTarget(GameObject target)
    {
        Quaternion targetRotation;
        if (target != null)
        {
            Vector3 direction = (target.transform.position + new Vector3(0, 0.5f, 0)) - transform.position;
            targetRotation = Quaternion.LookRotation(direction);

            // Calculate the angle between the turret and the target
            float angle = Quaternion.Angle(transform.rotation, targetRotation);

            // If the angle is greater than the allowed angle, clamp the rotation
            if (angle > maxRotationAngle)
            {
                targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationAngle);
            }
        }
        else
        {
            targetRotation = Quaternion.LookRotation(startPosition - transform.position);
        }

        // Rotate the turret towards the target
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void Shoot(GameObject target)
    {
        if (Time.time > nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            TurrentBullet turret = bullet.GetComponent<TurrentBullet>();
            turret.damage = bulletDmg;

            nextFireTime = Time.time + 5f; // set nextFireTime to five seconds in the future
        }
    }
}