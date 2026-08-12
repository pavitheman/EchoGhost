using UnityEngine;

public class SimpleSpriteAnimator : MonoBehaviour
{
    public Sprite[] idleFrames;
    public Sprite[] walkFrames;
    public Sprite[] jumpFrames;
    public float frameRate = 0.15f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private int currentFrame;
    private float timer;
    private int lastState = -1; // 0 = idle, 1 = walk, 2 = jump

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f;
        bool isJumping = Mathf.Abs(rb.linearVelocity.y) > 0.1f;

        int state = isJumping ? 2 : (isMoving ? 1 : 0);

        if (state != lastState)
        {
            currentFrame = 0;
            timer = 0f;
            lastState = state;
        }

        Sprite[] currentSet = state == 2 ? jumpFrames : (state == 1 ? walkFrames : idleFrames);
        if (currentSet.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % currentSet.Length;
            spriteRenderer.sprite = currentSet[currentFrame];
        }
    }
}