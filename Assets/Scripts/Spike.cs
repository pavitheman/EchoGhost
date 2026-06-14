using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Spike hit: " + other.gameObject.name + " tag: " + other.gameObject.tag);

        if (other.CompareTag("Player"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (other.CompareTag("Echo"))
        {
            PlayerController.echoActive = false;
            other.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            other.gameObject.GetComponent<Rigidbody2D>().simulated = false;
            EchoTrail trail = other.gameObject.GetComponent<EchoTrail>();
            if (trail != null) trail.SolidifyOnDeath();
        }
    }
}