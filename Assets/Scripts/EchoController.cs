using UnityEngine;
using UnityEngine.InputSystem;

public class EchoController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector3 spawnPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        rb.gravityScale = 1;

        // Save original spawn position
        spawnPosition = transform.position;

        BoxCollider2D triggerCol = gameObject.AddComponent<BoxCollider2D>();
        triggerCol.isTrigger = true;
        triggerCol.size = new Vector2(0.8f, 0.8f);
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            PlayerController.echoActive = !PlayerController.echoActive;
            rb.simulated = PlayerController.echoActive;
            GetComponent<SpriteRenderer>().enabled = true;
            isGrounded = false;

            if (PlayerController.echoActive)
            {
                // Reset echo to spawn position
                transform.position = spawnPosition;
                rb.linearVelocity = Vector2.zero;
            }
        }

        if (!PlayerController.echoActive) return;

        float move = 0;
        if (Keyboard.current.aKey.isPressed) move = -1;
        if (Keyboard.current.dKey.isPressed) move = 1;
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Contains("TrailSegment")) return;
        isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name.Contains("TrailSegment")) return;
        isGrounded = false;
    }
}