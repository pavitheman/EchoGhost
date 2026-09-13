using UnityEngine;
using UnityEngine.InputSystem;

public class DoorButton : MonoBehaviour
{
    public Door targetDoor;
    public Sprite idleSprite;
    public Sprite pressedSprite;
    [Tooltip("If false, the Echo cannot activate this switch -- only the real Player can. Also blocks pressing while the Player is off exploring in Echo mode.")]
    public bool allowEcho = true;
    private SpriteRenderer sr;
    private bool playerNearby = false;
    private bool isPressed = false;
    private AudioClip activateClip;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite;
        activateClip = Resources.Load<AudioClip>("Audio/switch_activate");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || (allowEcho && other.CompareTag("Echo")))
            playerNearby = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || (allowEcho && other.CompareTag("Echo")))
            playerNearby = false;
    }

    void Update()
    {
        // While Echo mode is off-limits for this switch, block presses
        // entirely whenever the player is currently riding Echo -- this also
        // covers the real Player's body sitting idle on the trigger zone
        // (left behind while controlling Echo elsewhere), not just Echo's
        // own collider.
        if (!allowEcho && PlayerController.echoActive) return;

        // Deliberately NOT the E/interact key -- this switch only responds
        // to Up Arrow, so E can never activate it.
        if (playerNearby && !isPressed && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isPressed = true;
            sr.sprite = pressedSprite;
            targetDoor.SetOpen(true);
            if (activateClip != null)
                AudioSource.PlayClipAtPoint(activateClip, transform.position, 0.8f);
        }
    }
}
