using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    public static bool echoActive = false;
    public static bool nearTotem = false;

    private Vector3 checkpoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        checkpoint = transform.position;
    }

    void Update()
    {
        if (echoActive) return;

        float move = 0;
        if (Keyboard.current.leftArrowKey.isPressed) move = -1;
        if (Keyboard.current.rightArrowKey.isPressed) move = 1;
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (Keyboard.current.zKey.wasPressedThisFrame && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    public void SetCheckpoint(Vector3 pos)
    {
        checkpoint = pos;
    }

    public void Die()
    {
        transform.position = checkpoint;
        rb.linearVelocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        isGrounded = true;
    }
}