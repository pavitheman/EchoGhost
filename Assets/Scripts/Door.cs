using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private Collider2D col;
    private SpriteRenderer sr;
    public Sprite closedSprite;
    public Sprite openSprite;
    public bool IsOpen { get; private set; }

    [Header("Sinking Pillar Mode")]
    [Tooltip("If true, opening this Door slowly sinks it straight down into the ground instead of instantly swapping sprites and disabling its collider.")]
    public bool sinkWhenOpened = false;
    public float sinkDistance = 6f;
    public float sinkSpeed = 1.5f; // world units per second

    private Vector3 closedPosition;
    private Coroutine sinkRoutine;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedSprite;
        closedPosition = transform.position;
    }

    public void SetOpen(bool open)
    {
        IsOpen = open;

        if (sinkWhenOpened)
        {
            if (sinkRoutine != null) StopCoroutine(sinkRoutine);
            sinkRoutine = StartCoroutine(SinkRoutine(open));
        }
        else
        {
            col.enabled = !open;
            sr.sprite = open ? openSprite : closedSprite;
        }
    }

    private IEnumerator SinkRoutine(bool open)
    {
        Vector3 target = open ? closedPosition + Vector3.down * sinkDistance : closedPosition;

        while (Vector3.Distance(transform.position, target) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, sinkSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;

        // Only block the path while the pillar is still up.
        if (col != null) col.enabled = !open;
    }
}
