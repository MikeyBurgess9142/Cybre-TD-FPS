using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentBullet : MonoBehaviour
{
  
        public float speed = 10f;
        public int damage = 1;
        public float lifetime = 5f;
    public float detectionDistance = 1f;

    private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void Update()
        {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Check for enemies in front of the bullet using a Raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                // Assuming the enemy has a script called "EnemyHealth" that manages its health
                EnemyTurrentDamager enemyHealth = hit.collider.GetComponent<EnemyTurrentDamager>();
                ZombieAI zombieAI = hit.collider.GetComponent<ZombieAI>();
                BossTurrentDamager bossHealth = hit.collider.GetComponent<BossTurrentDamager>();

                if (enemyHealth != null)
                {
                    enemyHealth.CallTakeDamage(damage);
                }
                if (bossHealth != null)
                {
                    bossHealth.CallTakeDamage(damage);
                }
                if (zombieAI != null)
                {
                    zombieAI.TakeDamage(damage);
                }

                Destroy(gameObject);
            }
        }
    }
}
    

