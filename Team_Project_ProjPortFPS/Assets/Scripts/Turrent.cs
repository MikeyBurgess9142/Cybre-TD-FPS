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

    [Header("-----Bullet-----")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 1f;
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
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void RotateTowardsTarget(GameObject target)
    {
        Vector3 direction = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void Shoot(GameObject target)
    {
        if (Time.time > nextFireTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            TurrentBullet turret = bullet.GetComponent<TurrentBullet>();
            turret.damage = bulletDmg;

            bullet.GetComponent<Rigidbody>().velocity = firePoint.forward * bulletSpeed;
            nextFireTime = Time.time + 1f / fireRate;
        }
    }
}
