using UnityEngine;
using UnityEngine.InputSystem;

public class EchoController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 9f;
    public bool isDoubleJumping = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private Vector3 spawnPosition;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.simulated = false;
        rb.gravityScale = 1;

        spawnPosition = transform.position;

        BoxCollider2D triggerCol = GetComponent<BoxCollider2D>();

        if (triggerCol == null)
        {
            triggerCol = gameObject.AddComponent<BoxCollider2D>();
        }

        triggerCol.isTrigger = false;

        spriteRenderer.enabled = false;

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
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            bool turningOn = !PlayerController.echoActive;

            if (turningOn && !PlayerController.nearTotem)
            {
                Debug.Log("E pressed but player is not near the portal.");
                return;
            }

            PlayerController.echoActive = !PlayerController.echoActive;

            rb.simulated = PlayerController.echoActive;
            spriteRenderer.enabled = PlayerController.echoActive;

            if (PlayerController.echoActive)
            {
                transform.position = spawnPosition;
                rb.linearVelocity = Vector2.zero;
                jumpCount = 0;
                isGrounded = false;
                isDoubleJumping = false;
            }
            else
            {
                isDoubleJumping = false;
            }
        }

        if (!PlayerController.echoActive)
            return;

        float move = 0;

        if (Keyboard.current.aKey.isPressed)
            move = -1;

        if (Keyboard.current.dKey.isPressed)
            move = 1;

        rb.linearVelocity = new Vector2(
            move * moveSpeed,
            rb.linearVelocity.y
        );

        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

                jumpCount = 1;
                isGrounded = false;
                isDoubleJumping = false;
            }
            else if (jumpCount < maxJumps)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

                jumpCount++;
                isDoubleJumping = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name.Contains("TrailSegment"))
            return;

        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpCount = 0;
                isDoubleJumping = false;
                break;
            }
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name.Contains("TrailSegment"))
            return;

        isGrounded = false;
    }
}