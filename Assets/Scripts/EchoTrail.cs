using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoTrail : MonoBehaviour
{
    private LineRenderer lr;
    private List<Vector2> points = new List<Vector2>();
    public float minDistance = 0.5f;
    public float trailThickness = 0.2f;
    private bool wasActive = false;
    private List<GameObject> colliderObjects = new List<GameObject>();

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    void Update()
    {
        if (PlayerController.echoActive)
        {
            // Clear old trail when redeploying
            if (!wasActive)
            {
                points.Clear();
                lr.positionCount = 0;
                foreach (var obj in colliderObjects)
                    if (obj != null) Destroy(obj);
                colliderObjects.Clear();
            }

            wasActive = true;
            Vector2 currentPos = transform.position;

            if (points.Count == 0 || Vector2.Distance(currentPos, points[points.Count - 1]) >= minDistance)
            {
                points.Add(currentPos);
                lr.positionCount = points.Count;
                for (int i = 0; i < points.Count; i++)
                    lr.SetPosition(i, points[i]);
            }
        }
        else if (wasActive)
        {
            wasActive = false;
            SolidifyTrail();
        }
    }

    void SolidifyTrail()
    {
        if (points.Count < 2) return;

        foreach (var obj in colliderObjects)
            if (obj != null) Destroy(obj);
        colliderObjects.Clear();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[i + 1];
            Vector2 mid = (a + b) / 2f;
            float angle = Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
            float length = Vector2.Distance(a, b);

            GameObject seg = new GameObject("TrailSegment");
            seg.transform.position = mid;
            seg.transform.rotation = Quaternion.Euler(0, 0, angle);
            BoxCollider2D col = seg.AddComponent<BoxCollider2D>();
            col.size = new Vector2(length, trailThickness);
            colliderObjects.Add(seg);
        }

        Debug.Log("Trail solidified with " + colliderObjects.Count + " segments");
    }
}