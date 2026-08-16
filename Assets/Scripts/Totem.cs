using UnityEngine;

public class Totem : MonoBehaviour
{
    public Sprite unlitSprite;
    public Sprite litSprite;

    private bool activated = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (unlitSprite != null)
            sr.sprite = unlitSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController.nearTotem = true;

        if (!activated)
        {
            EchoController echo = FindFirstObjectByType<EchoController>();
            if (echo != null)
            {
                echo.SetSpawnPoint(other.transform.position);
            }

            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.SetCheckpoint(other.transform.position);
            }

            activated = true;
            if (litSprite != null)
                sr.sprite = litSprite;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        PlayerController.nearTotem = false;
    }
}