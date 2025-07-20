using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;
    public float despawnTime = 3f;
    public float speed = 60f;
    public float sphereRadius = 0.3f;
    public LayerMask hitMask; // Assign in Inspector: Include "Player", exclude "Enemy"

    private Vector3 velocity;

    void Start()
    {
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
                PlayerStats player = hit.collider.GetComponent<PlayerStats>();
                if (player != null)
                {
                    player.TakeDamage(damage);
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