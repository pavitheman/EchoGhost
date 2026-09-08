using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public bool facingLeft = false;
    public Sprite idleSprite;
    public Sprite firingSprite;
    public float flashDuration = 0.15f;
    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (idleSprite != null)
            sr.sprite = idleSprite;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= fireRate)
        {
            timer = 0f;
            Fire();
        }
    }

    void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
        laser.GetComponent<LaserProjectile>().SetDirection(facingLeft ? Vector2.left : Vector2.right);

        StartCoroutine(FlashFiringSprite());
    }

    System.Collections.IEnumerator FlashFiringSprite()
    {
        if (firingSprite != null) sr.sprite = firingSprite;
        yield return new WaitForSeconds(flashDuration);
        if (idleSprite != null) sr.sprite = idleSprite;
    }
}