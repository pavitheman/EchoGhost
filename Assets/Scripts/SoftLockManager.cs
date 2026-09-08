using UnityEngine;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;
    public Vector3 spawnPoint;
    private List<Vector3> activatedCheckpoints = new List<Vector3>();

    void Awake()
    {
        Instance = this;
    }

    public void RegisterCheckpoint(Vector3 pos)
    {
        if (!activatedCheckpoints.Contains(pos))
            activatedCheckpoints.Add(pos);
    }

    public Vector3 GetNearestCheckpoint(Vector3 currentPos)
    {
        Vector3 best = spawnPoint;
        float bestDist = Vector3.Distance(currentPos, spawnPoint);

        foreach (var cp in activatedCheckpoints)
        {
            float d = Vector3.Distance(currentPos, cp);
            if (d < bestDist)
            {
                bestDist = d;
                best = cp;
            }
        }
        return best;
    }
}