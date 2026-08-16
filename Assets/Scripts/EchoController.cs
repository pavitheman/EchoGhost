using UnityEngine;
using UnityEngine.InputSystem;

public class EchoController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 6f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector3 spawnPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Echo starts inactive
        rb.simulated = false;
        rb.gravityScale = 1;

        // Save its initial position as a fallback
        spawnPosition = transform.position;

        // Make sure the Echo is invisible at the start
        spriteRenderer.enabled = false;

        // Ignore collision with Player
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Collider2D echoCollider = GetComponent<Collider2D>();
            Collider2D playerCollider = player.GetComponent<Collider2D>();

            if (echoCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(echoCollider, playerCollider);
            }
        }
    }

    public void SetSpawnPoint(Vector3 newSpawn)
    {
        spawnPosition = newSpawn;

        Debug.Log("Echo spawn point set to: " + spawnPosition);
    }

    void Update()
    {
        // Press E
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Only allow activation near the portal
            if (!PlayerController.echoActive && !PlayerController.nearTotem)
            {
                Debug.Log("E pressed, but player is not near the portal.");
                return;
            }

            // Toggle Echo
            PlayerController.echoActive = !PlayerController.echoActive;

            // Turn physics on/off
            rb.simulated = PlayerController.echoActive;

            // Turn sprite on/off
            spriteRenderer.enabled = PlayerController.echoActive;

            isGrounded = false;

            if (PlayerController.echoActive)
            {
                // Spawn AT THE PORTAL
                transform.position = spawnPosition;

                // Stop movement
                rb.linearVelocity = Vector2.zero;

                Debug.Log("ECHO SPAWNED AT PORTAL: " + transform.position);
                Debug.Log("Sprite visible: " + spriteRenderer.enabled);
            }
            else
            {
                Debug.Log("Echo turned off.");
            }
        }

        // Don't move if Echo is inactive
        if (!PlayerController.echoActive)
            return;

        // Horizontal movement
        float move = 0f;

        if (Keyboard.current.aKey.isPressed)
            move = -1f;

        if (Keyboard.current.dKey.isPressed)
            move = 1f;

        rb.linearVelocity = new Vector2(
            move * moveSpeed,
            rb.linearVelocity.y
        );

        // Jump
        if (Keyboard.current.wKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            isGrounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Contains("TrailSegment"))
            return;

        isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name.Contains("TrailSegment"))
            return;

        isGrounded = false;
    }
}