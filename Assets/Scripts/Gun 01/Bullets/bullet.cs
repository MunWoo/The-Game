using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public int damage;
    public int despawnTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, despawnTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the bullet collided with an enemy (assuming the enemy has a tag called "Enemy")
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the enemy's script and call a method to apply damage
            TestEnemy enemy = collision.gameObject.GetComponent<TestEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);  // Apply damage to the enemy
            }
            // Destroy the bullet after it hits the enemy (optional)
            Destroy(gameObject);
        }
    }
}
