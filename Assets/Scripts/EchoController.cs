using UnityEngine;
using UnityEngine.InputSystem;

public class EchoController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 15f;
    public bool isDoubleJumping = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount = 0;
    private int maxJumps = 2;
    private Vector3 spawnPosition;
    private SpriteRenderer spriteRenderer;

    // --- Audio ---
    private AudioSource sfx;      // one-shot sounds: jump, land, footstep, toggle
    private AudioSource hum;      // looping ambient echo hum while active
    private AudioClip jumpClip;
    private AudioClip landClip;
    private AudioClip footstepClip;
    private AudioClip toggleClip;
    private AudioClip humClip;
    public float footstepInterval = 0.28f;
    private float footstepTimer = 0f;

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

        sfx = gameObject.AddComponent<AudioSource>();
        sfx.playOnAwake = false;

        hum = gameObject.AddComponent<AudioSource>();
        hum.playOnAwake = false;
        hum.loop = true;
        hum.volume = 0.35f;

        jumpClip = Resources.Load<AudioClip>("Audio/jump");
        landClip = Resources.Load<AudioClip>("Audio/land");
        footstepClip = Resources.Load<AudioClip>("Audio/footstep");
        toggleClip = Resources.Load<AudioClip>("Audio/echo_toggle");
        humClip = Resources.Load<AudioClip>("Audio/echo_hum");
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

            if (toggleClip != null) sfx.PlayOneShot(toggleClip);

            rb.simulated = PlayerController.echoActive;
            spriteRenderer.enabled = PlayerController.echoActive;

            if (PlayerController.echoActive)
            {
                transform.position = spawnPosition;
                rb.linearVelocity = Vector2.zero;
                jumpCount = 0;
                isGrounded = false;
                isDoubleJumping = false;

                if (humClip != null)
                {
                    hum.clip = humClip;
                    hum.Play();
                }
            }
            else
            {
                isDoubleJumping = false;
                if (hum.isPlaying) hum.Stop();
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

        // Footstep loop while grounded and moving
        if (isGrounded && Mathf.Abs(move) > 0.01f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (footstepClip != null) sfx.PlayOneShot(footstepClip, 0.5f);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

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
                if (jumpClip != null) sfx.PlayOneShot(jumpClip, 0.75f);
            }
            else if (jumpCount < maxJumps)
            {
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    jumpForce
                );

                jumpCount++;
                isDoubleJumping = true;
                if (jumpClip != null) sfx.PlayOneShot(jumpClip, 0.6f);
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
                if (!isGrounded && landClip != null)
                {
                    sfx.PlayOneShot(landClip, 0.6f);
                }
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
