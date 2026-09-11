using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private Vector2 startPos;
    private float length;
    private Camera cam;

    public float ParallaxAmountX;
    public float ParallaxAmountY;
    public bool loop = true;

    private SpriteRenderer SpriteRenderer;
    private Bounds spriteBounds;
    public Vector2 camStartPos;


    void Start()
    {
        cam = Camera.main;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        spriteBounds = SpriteRenderer.localBounds;

        startPos = transform.position;
        length = spriteBounds.size.x;
    }

    void Update()
    {
        float distanceX = (cam.transform.position.x - camStartPos.x) * ParallaxAmountX;
        float distanceY = (cam.transform.position.y - camStartPos.y) * ParallaxAmountY;

        transform.position = new Vector2(startPos.x + distanceX, startPos.y + distanceY);

        float movementX = (cam.transform.position.x - camStartPos.x) * (1 - ParallaxAmountX);

        if (loop)
        {
            if (movementX > startPos.x + length)
            {
                startPos.x = length;
            }
            else if (movementX < startPos.x - length)
            {
                startPos.x -= length;
            }
        }
    }
}
