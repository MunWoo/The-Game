using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;
    public float despawnTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, despawnTime);
    }
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Colided with " + collision.gameObject.name);
        // Ignore collision with objects tagged "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with enemy and passed throough");
            return; // Do nothing, bullet passes through Enemy objects
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Found Player");
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Took damage");
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Didnt colide");
        }
    }
}

