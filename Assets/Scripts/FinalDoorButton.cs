using UnityEngine;
using UnityEngine.InputSystem;

public class FinalDoorButton : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite pressedSprite;
    public ButtonSequence memoryGame;
    private SpriteRenderer sr;
    private bool playerNearby = false;
    private bool isPressed = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Echo"))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Echo"))
            playerNearby = false;
    }

    void Update()
    {
        if (playerNearby && !isPressed && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isPressed = true;
            sr.sprite = pressedSprite;
            memoryGame.StartGame();
        }
    }
}