using UnityEngine;

public class Spike : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Spike hit: " + other.gameObject.name + " tag: " + other.gameObject.tag);

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
                PlayHitSound();
            }
        }

        if (other.CompareTag("Echo"))
        {
            PlayerController.echoActive = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            EchoTrail trail = other.gameObject.GetComponent<EchoTrail>();
            if (trail != null) trail.SolidifyOnDeath();
            PlayHitSound();
        }
    }

    void PlayHitSound()
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/spike_hit");
        if (clip != null)
        {
            // Use a temporary audio source at this position so the sound
            // survives even if the object it's attached to is disabled/reset.
            AudioSource.PlayClipAtPoint(clip, transform.position, 1f);
        }
    }
}
