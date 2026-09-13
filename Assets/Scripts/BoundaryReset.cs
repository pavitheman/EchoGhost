using UnityEngine;

public class BoundaryReset : MonoBehaviour
{
 void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Boundary hit: " + other.gameObject.name + " tag: " + other.gameObject.tag);

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Die();
            }
        }

        if (other.CompareTag("Echo"))
        {
            PlayerController.DeathCount++;
            PlayerController.echoActive = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            EchoTrail trail = other.gameObject.GetComponent<EchoTrail>();
            if (trail != null) trail.SolidifyOnDeath();
        }
    }
}
