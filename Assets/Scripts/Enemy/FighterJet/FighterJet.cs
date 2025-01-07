using UnityEngine;
using UnityEngine.UI;

public class FighterJet : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float speed = 15f; // Movement speed
    [SerializeField] private int maxHealth = 1; // Maximum health (1-shot kill)
    [SerializeField] private GameObject bulletPrefab; // Prefab for the bullets
    [SerializeField] private Transform gunPoint; // Position where bullets are fired
    [SerializeField] private float fireRate = 0.1f; // Time between bullet shots
    [SerializeField] private GameObject explosionEffect; // Explosion VFX prefab
    [SerializeField] private float rotationSpeed = 2f; // Speed of rotation towards the player
    [SerializeField] private Vector3 lowerHeight = new Vector3(93.25f, 12.39f, 54.69f); // Lower dive height
    [SerializeField] private Vector3 upperHeight = new Vector3(93.25f, 46.39f, 64.72f); // Level-off height
    [SerializeField] private float pullUpSpeed = 20f; // Speed during pull-up
    [SerializeField] private float loopDelay = 2f; // Time before returning to attack
    [SerializeField] private float exitTime = 5f; // Time in seconds before destroying the jet after starting exit
    [SerializeField] private int maxAttackCycles = 3; // Maximum number of attack cycles before exiting
    [SerializeField] private Slider healthBar; // Health bar slider
    [SerializeField] private Image fillImage; // Image component of the health bar fill area
    [SerializeField] private GameObject coinPrefab; // Prefab for coins
    [SerializeField] private int numberOfCoins = 5; // Number of coins to drop

    private Transform playerTransform; // Reference to the player
    private int currentHealth;
    private float fireTimer; // Timer to track firing intervals
    private bool pullingUp = false; // Track if the jet is pulling up
    private bool hasPulledUp = false; // Track if the jet has reached the lower height and is eligible to level off
    private bool isReturning = false; // Whether the jet is returning to attack
    private bool isExiting = false; // Whether the jet is exiting the scene
    private bool isLevelingOffForExit = false; // Whether the jet is leveling off before exiting
    private float exitPreparationTime = 0f; // Track time spent in preparing to exit
    private Vector3 flyOverTarget; // Target position above the player
    private int attackCycleCount = 0; // Track the number of attack cycles completed

    private void Start()
    {
        Debug.Log("Fighter jet initialized.");
        currentHealth = maxHealth;

        // Set up health bar
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            if (fillImage != null)
            {
                fillImage.color = Color.green; // Start with green
            }
        }

        // Find the player in the scene
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("Player not found!");
            }
        }

        UpdateFlyOverTarget();
    }

    private void Update()
    {
        if (isExiting)
        {
            Debug.Log("Fighter jet is exiting.");
            if (isLevelingOffForExit)
            {
                Debug.Log("Fighter jet leveling off for exit.");
                LevelOffForExit();
            }
            return;
        }

        if (isReturning)
        {
            Debug.Log("Fighter jet is returning to attack.");
            HandleReturnToAttack();
        }
        else if (!pullingUp)
        {
            Debug.Log("Fighter jet attacking.");
            HandleAttack();
        }
        else
        {
            Debug.Log("Fighter jet pulling up.");
            if (!hasPulledUp)
            {
                PullUp();
            }
            else
            {
                LevelOff();
            }
        }
    }

    private void HandleAttack()
    {
        if (playerTransform != null)
        {
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            FireBullets();
            fireTimer = 0f;
        }

        if (transform.position.y <= lowerHeight.y)
        {
            Debug.Log("Fighter jet reached dive height. Initiating pull-up.");
            transform.position = new Vector3(transform.position.x, lowerHeight.y, transform.position.z);
            StartPullUp();
        }
    }

    private void FireBullets()
    {
        if (gunPoint != null && bulletPrefab != null)
        {
            Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
            Debug.Log("Fighter jet fired a bullet.");
        }
    }

    private void StartPullUp()
    {
        pullingUp = true;
    }

    private void PullUp()
    {
        Vector3 upwardDirection = transform.forward + Vector3.up * 0.5f;
        upwardDirection.Normalize();
        Quaternion pullUpRotation = Quaternion.LookRotation(upwardDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, pullUpRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * pullUpSpeed * Time.deltaTime);

        if (transform.position.y >= upperHeight.y)
        {
            Debug.Log("Fighter jet completed pull-up. Leveling off.");
            hasPulledUp = true;
        }
    }

    private void LevelOff()
    {
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0;
        Quaternion levelOffRotation = Quaternion.LookRotation(forwardDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, levelOffRotation, rotationSpeed * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        attackCycleCount++;
        if (attackCycleCount < maxAttackCycles)
        {
            Debug.Log($"Fighter jet completing cycle {attackCycleCount}. Returning to attack.");
            StartCoroutine(ReturnToAttack());
        }
        else
        {
            Debug.Log("Fighter jet reached max cycles. Preparing to exit.");
            isLevelingOffForExit = true;
            exitPreparationTime = 0f; // Reset the timer when preparing to exit
        }
    }

    private void LevelOffForExit()
    {
        Debug.Log("Fighter jet starting level-off for exit.");
        exitPreparationTime += Time.deltaTime;

        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0; // Level out the pitch
        Quaternion levelOffRotation = Quaternion.LookRotation(forwardDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, levelOffRotation, rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, levelOffRotation) < 1f)
        {
            Debug.Log("Fighter jet leveled off for exit. Starting exit countdown.");
            isLevelingOffForExit = false;
            StartCoroutine(ExitAfterTime());
        }
        else if (exitPreparationTime > 5f) // Timeout condition
        {
            Debug.LogWarning("Fighter jet stuck in exit preparation. Forcing exit.");
            isLevelingOffForExit = false;
            StartCoroutine(ExitAfterTime());
        }
    }

    private System.Collections.IEnumerator ExitAfterTime()
    {
        Debug.Log("Fighter jet starting exit countdown.");
        yield return new WaitForSeconds(exitTime);
        Debug.Log("Fighter jet exited. Destroying object.");
        Destroy(gameObject);
    }

    private void HandleReturnToAttack()
    {
        if (playerTransform != null)
        {
            UpdateFlyOverTarget();
            Vector3 directionToTarget = flyOverTarget - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, flyOverTarget) < 10f)
            {
                Debug.Log("Fighter jet returned to attack position.");
                isReturning = false;
                pullingUp = false;
                hasPulledUp = false;
            }
        }
    }

    private System.Collections.IEnumerator ReturnToAttack()
    {
        isReturning = true;
        yield return new WaitForSeconds(loopDelay);
    }

    private void UpdateFlyOverTarget()
    {
        if (playerTransform != null)
        {
            flyOverTarget = new Vector3(playerTransform.position.x, upperHeight.y, playerTransform.position.z);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
            UpdateHealthBarColor();
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Fighter jet destroyed by damage.");
            Die();
        }
    }

    private void UpdateHealthBarColor()
    {
        if (fillImage != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;

            if (healthPercentage > 0.5f)
            {
                fillImage.color = Color.green;
            }
            else if (healthPercentage > 0.2f)
            {
                fillImage.color = Color.yellow;
            }
            else
            {
                fillImage.color = Color.red;
            }
        }
    }

    private void Die()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        DropCoins();

        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        Destroy(gameObject, 5f);
    }

    private void DropCoins()
    {
        if (coinPrefab == null) return;

        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector3 dropPosition = transform.position + new Vector3(
                Random.Range(-1f, 1f),
                0.5f,
                Random.Range(-1f, 1f)
            );

            Instantiate(coinPrefab, dropPosition, Quaternion.identity);
        }

        Debug.Log($"{numberOfCoins} coins dropped by Fighter Jet!");
    }
}
