using UnityEngine;

public class Door : MonoBehaviour
{
    private Collider2D col;
    private SpriteRenderer sr;
    public Sprite closedSprite;
    public Sprite openSprite;
    public bool IsOpen { get; private set; }

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;
    }

    public void SetOpen(bool open)
    {
        IsOpen = open;
        col.enabled = !open;
        sr.sprite = open ? openSprite : closedSprite;
    }
}