using UnityEngine;
using UnityEngine.InputSystem;

public class DoorTeleporter : MonoBehaviour
{
    public Transform destination;
    public Door door;
    public float cooldown = 0.5f;
    [HideInInspector] public float lastTeleportTime = -10f;
    private bool playerNearby = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }

    void Update()
    {
        if (!playerNearby) return;
        if (door != null && !door.IsOpen) return;
        if (Time.time - lastTeleportTime < cooldown) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            GameObject.FindWithTag("Player").transform.position = destination.position;

            if (destination.TryGetComponent<DoorTeleporter>(out var otherTeleporter))
                otherTeleporter.lastTeleportTime = Time.time;

            lastTeleportTime = Time.time;
        }
    }
}