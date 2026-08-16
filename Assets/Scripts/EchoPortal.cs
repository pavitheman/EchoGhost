using UnityEngine;

public class EchoPortal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController.nearTotem = true;

        EchoController echo = FindFirstObjectByType<EchoController>();

        if (echo != null)
        {
            // Set the Echo spawn position to the portal
            echo.SetSpawnPoint(transform.position);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController.nearTotem = false;
    }
}