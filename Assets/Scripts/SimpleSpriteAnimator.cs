using UnityEngine;

public class SimpleSpriteAnimator : MonoBehaviour
{
    public Sprite[] idleFrames;
    public Sprite[] walkFrames;
    public Sprite[] jumpFrames;
    public Sprite[] doubleJumpFrames;
    public float frameRate = 0.15f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private EchoController echoController;
    private int currentFrame;
    private float timer;
    private int lastState = -1;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        echoController = GetComponent<EchoController>();
    }

    void Update()
    {
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = Mathf.Abs(rb.linearVelocity.y) > 0.1f;

        if (rb.linearVelocity.x > 0.1f)
            spriteRenderer.flipX = false;
        else if (rb.linearVelocity.x < -0.1f)
            spriteRenderer.flipX = true;

        int state;

        if (echoController != null && echoController.isDoubleJumping)
            state = 3;
        else if (isJumping)
            state = 2;
        else if (isMoving)
            state = 1;
        else
            state = 0;

        if (state != lastState)
        {
            currentFrame = 0;
            timer = 0f;
            lastState = state;

            Sprite[] newSet = GetAnimationSet(state);

            if (newSet != null && newSet.Length > 0)
                spriteRenderer.sprite = newSet[0];
        }

        Sprite[] currentSet = GetAnimationSet(state);

        if (currentSet == null || currentSet.Length == 0)
            return;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame++;

            if (currentFrame >= currentSet.Length)
                currentFrame = 0;

            spriteRenderer.sprite = currentSet[currentFrame];
        }
    }

    Sprite[] GetAnimationSet(int state)
    {
        switch (state)
        {
            case 0:
                return idleFrames;
            case 1:
                return walkFrames;
            case 2:
                return jumpFrames;
            case 3:
                return doubleJumpFrames;
            default:
                return idleFrames;
        }
    }
}