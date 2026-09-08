using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float lifetime = 5f;
    private Vector2 direction = Vector2.right;

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
        if (direction.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 respawn = CheckpointManager.Instance.GetNearestCheckpoint(other.transform.position);
            other.transform.position = respawn;

            if (other.TryGetComponent<Rigidbody2D>(out var rb))
                rb.linearVelocity = Vector2.zero;

            Destroy(gameObject);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}