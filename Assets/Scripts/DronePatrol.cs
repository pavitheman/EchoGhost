using UnityEngine;

public class DronePatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;
    private Vector3 target;
    private SpriteRenderer sr;

    void Start()
    {
        target = pointB.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 prevPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        float direction = transform.position.x - prevPosition.x;
        if (direction > 0.001f)
            sr.flipX = false;
        else if (direction < -0.001f)
            sr.flipX = true;

        if (Vector3.Distance(transform.position, target) < 0.05f)
        {
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Vector3 respawn = CheckpointManager.Instance.GetNearestCheckpoint(other.transform.position);
        other.transform.position = respawn;

        if (other.TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = Vector2.zero;
    }
}