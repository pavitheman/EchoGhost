using UnityEngine;

public class DroneAnimator : MonoBehaviour
{
    public Sprite[] walkFrames;
    public float frameRate = 0.15f;
    private SpriteRenderer sr;
    private int currentFrame;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % walkFrames.Length;
            sr.sprite = walkFrames[currentFrame];
        }
    }
}