using UnityEngine;
using UnityEngine.InputSystem;

public class SoftReset : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Vector3 target = CheckpointManager.Instance.GetNearestCheckpoint(transform.position);
            transform.position = target;
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }
    }
}