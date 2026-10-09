using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 4.0f;

    private Rigidbody enemyRb;
    private GameObject player;

    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // FixedUpdate runs at a steady rate, so the push is the same at any frame rate
    void FixedUpdate()
    {
        // Chases the player around the island
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);
    }

    void Update()
    {
        // Destroys the enemy if it falls off the island
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}