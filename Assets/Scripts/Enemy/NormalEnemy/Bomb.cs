using UnityEngine;
using EZCameraShake;
using UnityEngine.SceneManagement;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 5f; // Radius of the explosion
    [SerializeField] private float explosionForce = 500f; // Force applied by the explosion
    [SerializeField] private int maxDamage = 50; // Maximum damage dealt to the player
    [SerializeField] private GameObject explosionEffect; // Explosion VFX prefab
    [SerializeField] private float lifetime = 5f; // Time before the bomb is destroyed
    [SerializeField] private AudioClip explosionSound; // Explosion sound effect
    [SerializeField] private AudioSource audioSource; // AudioSource to play the sound

    private bool hasExploded = false;
    private bool isMainMenu = false;

    void Start()
    {
        // Determine if the current scene is the MainMenu
        isMainMenu = SceneManager.GetActiveScene().name == "MainMenu";

        Debug.Log($"Bomb initialized in scene: {SceneManager.GetActiveScene().name}");

        // Debugging for AudioSource and ExplosionSound
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned to the bomb!");
        }

        if (explosionSound == null)
        {
            Debug.LogError("Explosion sound is not assigned to the bomb!");
        }

        // Destroy the bomb after a certain time
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Prevent multiple explosions
        if (hasExploded) return;
        hasExploded = true;

        Debug.Log("Bomb collided with something. Triggering explosion...");

        // Trigger the explosion
        Explode();
    }

    void Explode()
    {
        Debug.Log("Explode method called.");

        // Play the explosion sound using a temporary GameObject
        if (explosionSound != null)
        {
            Debug.Log("Playing explosion sound.");
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = transform.position; // Position it at the bomb's location
            AudioSource tempAudioSource = tempAudio.AddComponent<AudioSource>();
            tempAudioSource.clip = explosionSound;
            tempAudioSource.spatialBlend = 1.0f; // Ensure it's spatial (3D sound)
            tempAudioSource.Play();

            // Destroy the temporary GameObject after the sound finishes
            Destroy(tempAudio, explosionSound.length);
        }
        else
        {
            Debug.LogWarning("Explosion sound is not assigned.");
        }

        // Instantiate explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Debug.Log("Explosion effect instantiated.");
        }
        else
        {
            Debug.LogWarning("Explosion effect is not assigned.");
        }

        // Check for objects in the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        Debug.Log($"Explosion hit {colliders.Length} objects.");

        foreach (Collider nearbyObject in colliders)
        {
            // Apply explosion force if the object has a Rigidbody
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                Debug.Log($"Explosion force applied to {nearbyObject.name}.");
            }

            // Check if the object is the player
            PlayerHealth player = nearbyObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                if (!isMainMenu) // Only apply camera shake in gameplay scenes
                {
                    CameraShaker.Instance.ShakeOnce(20f, 20f, .1f, 1f);
                    Debug.Log("Camera shake triggered.");
                }

                float distance = Vector3.Distance(transform.position, player.transform.position);
                float damageFactor = Mathf.Clamp01(1 - (distance / explosionRadius));
                int damage = Mathf.RoundToInt(maxDamage * damageFactor);
                player.TakeDamage(damage);
                Debug.Log($"Player took {damage} damage from the bomb.");
            }
        }

        // Destroy the bomb immediately after triggering the explosion
        Destroy(gameObject);
        Debug.Log("Bomb object destroyed.");
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
