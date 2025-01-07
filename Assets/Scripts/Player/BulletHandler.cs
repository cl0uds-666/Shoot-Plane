using UnityEngine;

public class BulletHandler : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 75.0f; // Speed at which the bullet is launched
    [SerializeField] private GameObject bulletPrefab; // Prefab for the bullet
    [SerializeField] private Transform firePoint; // Position where the bullet spawns
    [SerializeField] private AudioClip fireSound; // Sound effect for firing
    [SerializeField] private AudioSource audioSource; // AudioSource for playing sounds
    [SerializeField] private float fireRate = 0.2f; // Time interval between individual shots in the burst
    [SerializeField] private int bulletDamage = 10; // Damage dealt by bullets
    [SerializeField] private float cooldownTime = 1f; // Cooldown time after burst ends

    private bool isFiring = false; // Is the player currently firing
    private bool isCoolingDown = false; // Is the player in cooldown
    private float nextFireTime = 0f; // Time for the next shot in the burst

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isCoolingDown) // Fire1 (Mouse Button 1)
        {
            StartFiring();
        }

        if (isFiring && Time.time >= nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void StartFiring()
    {
        if (isFiring || isCoolingDown) return;

        isFiring = true;

        // Play the firing sound
        if (audioSource != null && fireSound != null)
        {
            audioSource.clip = fireSound;
            audioSource.loop = false;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or FireSound is not assigned!");
        }

        // Schedule the end of the burst based on the sound length
        Invoke(nameof(StopFiring), fireSound.length);
    }

    private void FireBullet()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogError("Bullet prefab or fire point is not assigned!");
            return;
        }

        // Instantiate the bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Apply velocity to the bullet
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * launchSpeed;
        }
    }

    private void StopFiring()
    {
        isFiring = false;

        // Start the cooldown
        isCoolingDown = true;
        Invoke(nameof(ResetCooldown), cooldownTime);
        Debug.Log("Burst ended. Cooldown started.");
    }

    private void ResetCooldown()
    {
        isCoolingDown = false;
        Debug.Log("Cooldown ended. Ready to fire again.");
    }

    public void IncreaseBulletDamage(int amount)
    {
        bulletDamage += amount;
        Debug.Log($"Bullet Damage Increased. New Damage: {bulletDamage}");
    }

    public void DecreaseFireRate(float amount)
    {
        fireRate = Mathf.Max(0.05f, fireRate - amount); // Prevent fire rate from going too low
        Debug.Log($"Fire Rate Decreased. New Fire Rate: {fireRate}");
    }

    public int GetBulletDamage()
    {
        return bulletDamage;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public void ResetBulletDamage(int damage)
    {
        bulletDamage = damage;
        Debug.Log($"Bullet damage reset. New damage: {bulletDamage}");
    }

    public void ResetFireRate(float rate)
    {
        fireRate = rate;
        Debug.Log($"Fire rate reset. New rate: {fireRate}");
    }

}
