using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorTeleporter : MonoBehaviour
{
    public Door door;
    public string targetScene = "Level_2";
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

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
