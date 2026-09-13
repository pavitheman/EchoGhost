using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 31f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public static bool echoActive = false;
    public static bool nearTotem = false;

    // Total deaths (player + Echo) since the current scene loaded. Read by
    // DeathCounterUI to drive the on-screen counter.
    public static int DeathCount = 0;

    private Vector3 checkpoint;

    // --- Audio ---
    private AudioSource sfx;
    private AudioClip jumpClip;
    private AudioClip landClip;
    private AudioClip footstepClip;
    public float footstepInterval = 0.32f;
    private float footstepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkpoint = transform.position;

        sfx = GetComponent<AudioSource>();
        if (sfx == null) sfx = gameObject.AddComponent<AudioSource>();
        sfx.playOnAwake = false;

        jumpClip = Resources.Load<AudioClip>("Audio/jump");
        landClip = Resources.Load<AudioClip>("Audio/land");
        footstepClip = Resources.Load<AudioClip>("Audio/footstep");
    }

    void Update()
    {
        if (echoActive) return;

        float move = 0;
        if (Keyboard.current.leftArrowKey.isPressed) move = -1;
        if (Keyboard.current.rightArrowKey.isPressed) move = 1;
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        // Footstep loop while grounded and actually moving
        if (isGrounded && Mathf.Abs(move) > 0.01f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (footstepClip != null) sfx.PlayOneShot(footstepClip, 0.6f);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }

        if (Keyboard.current.zKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
            if (jumpClip != null) sfx.PlayOneShot(jumpClip, 0.75f);
        }
    }

    public void SetCheckpoint(Vector3 pos)
    {
        checkpoint = pos;
    }

    public void Die()
    {
        DeathCount++;
        transform.position = checkpoint;
        rb.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!isGrounded && landClip != null && Time.timeSinceLevelLoad > 0.3f)
        {
            sfx.PlayOneShot(landClip, 0.7f);
        }
        isGrounded = true;
    }
}
