using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrent : MonoBehaviour
{
    [Header("-----Turret-----")]
    public Transform firePoint;
    public float range = 10f;
    public float rotationSpeed = 5f;
    public float fireRate = 1f;
    private float nextFireTime = 0f;
    public float maxRotationAngle;
    [Header("-----Bullet-----")]
    public GameObject bulletPrefab;

    public int bulletDmg = 1;


    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Update()
    {
        RemoveDeadEnemies();
        GameObject target = FindClosestEnemy();

        if (target != null)
        {
            RotateTowardsTarget(target);
            Shoot(target);
           
        }
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
        Vector3 direction = (target.transform.position + new Vector3(0, 0.5f, 0)) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Calculate the angle between the turret and the target
        float angle = Quaternion.Angle(transform.rotation, targetRotation);

        // If the angle is greater than the allowed angle, clamp the rotation
        if (angle > maxRotationAngle)
        {
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationAngle);
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

          
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}
