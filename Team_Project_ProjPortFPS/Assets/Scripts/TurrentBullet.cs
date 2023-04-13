using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrentBullet : MonoBehaviour
{
  
        public float speed = 10f;
        public int damage = 1;
        public float lifetime = 5f;

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") )
            {

            // Assuming the enemy has a script called "EnemyHealth" that manages its health
            EnemyTurrentDamager enemyHealth = other.GetComponent<EnemyTurrentDamager>();
            BossTurrentDamager bossHealth = other.GetComponent<BossTurrentDamager>();
            if (enemyHealth != null)
            {
                enemyHealth.CallTakeDamage(damage);
                Destroy(gameObject);
            }
            if (bossHealth != null)
            {
                bossHealth.CallTakeDamage(damage);
                Destroy(gameObject);
            }
           
           
        }


        }
    
}
