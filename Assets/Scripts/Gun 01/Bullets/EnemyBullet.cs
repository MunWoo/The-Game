using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;
    public float despawnTime = 3f;
    public float speed = 60f;
    public float sphereRadius = 0.3f;
    public LayerMask hitMask; // Assign in Inspector: Include "Player", exclude "Enemy"
    public PlayerStats playerStats;

    private Vector3 velocity;

    void Start()
    {
        playerStats = PlayerStats.instance;
        velocity = transform.forward * speed;
        Destroy(gameObject, despawnTime);
    }
    public void Initialize(Vector3 initialVelocity)
    {
        velocity = initialVelocity;
    }
    void Update()
    {
        Vector3 move = velocity * Time.deltaTime;
        Vector3 direction = velocity.normalized;

        // SphereCast for collision
        if (Physics.SphereCast(transform.position, sphereRadius, direction, out RaycastHit hit, move.magnitude, hitMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (playerStats != null)
                {
                    playerStats.TakeDamage(damage);
                }
                else
                    Debug.Log("Couldnt find the Player");
            }

            Destroy(gameObject);
            return;
        }

        transform.position += move;

        // --- DEBUG DRAWING START ---
        // Draw ray line
        Debug.DrawRay(transform.position, direction * move.magnitude, Color.red, 1f);

        // Draw sphere at start
        DebugDrawSphere(transform.position, sphereRadius, Color.yellow);

        // Draw sphere at end
        DebugDrawSphere(transform.position + direction * move.magnitude, sphereRadius, Color.green);
        // --- DEBUG DRAWING END ---
    }
    void DebugDrawSphere(Vector3 center, float radius, Color color)
    {
        float step = 10f;
        for (float theta = 0; theta < 180f; theta += step)
        {
            for (float phi = 0; phi < 360f; phi += step)
            {
                float x = radius * Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad);
                float y = radius * Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
                float z = radius * Mathf.Cos(theta * Mathf.Deg2Rad);

                Vector3 point = center + new Vector3(x, y, z);
                Debug.DrawLine(center, point, color, 1f);
            }
        }
    }
}



/*
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
        // Ignore collision with objects tagged "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            return; // Do nothing, bullet passes through Enemy objects
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Didnt colide");
        }
    }
}
*/