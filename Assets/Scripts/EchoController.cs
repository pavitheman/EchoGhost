using UnityEngine;
using UnityEngine.InputSystem;

public class EchoController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isActive = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.47f, 0.87f, 0.5f);
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            isActive = !isActive;
            rb.simulated = isActive;
            Debug.Log("Echo deployed: " + isActive);
        }

        if (!isActive) return;

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
        isGrounded = true;
    }
}