using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float survivalTime = 120f;
    [SerializeField] private TextMeshProUGUI timerText; // Optional: only used in gameplay
    [SerializeField] private string gameOverScene = "GameOver";
    [SerializeField] private string victoryScene = "Victory";

    private bool isGameActive = true;
    private float remainingTime;
    private bool isCursorUnlocked = false; // Track whether the cursor is unlocked

    // Static kill count to persist across scenes
    public static int PlaneKillCount { get; private set; } = 0;

    private void Awake()
    {
        // Ensure only one GameManager exists across scenes
        GameManager[] managers = FindObjectsOfType<GameManager>();
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        remainingTime = survivalTime;

        // Handle behavior depending on the scene
        if (SceneManager.GetActiveScene().name == victoryScene)
        {
            UpdateKillText();
        }
        else
        {
            UpdateTimerUI();
            LockCursor(); // Lock cursor when gameplay starts
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == victoryScene)
        {
            return; // Skip gameplay updates
        }

        if (isGameActive)
        {
            HandleSurvivalTimer();
        }

        // Allow toggling cursor lock with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorUnlocked)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == victoryScene)
        {
            UpdateKillText();
        }
    }

    private void HandleSurvivalTimer()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            EndGame(true);
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateKillText()
    {
        TextMeshProUGUI planeKillText = FindObjectOfType<TextMeshProUGUI>();
        if (planeKillText != null && SceneManager.GetActiveScene().name == victoryScene)
        {
            planeKillText.text = $"Planes Destroyed: {PlaneKillCount}";
        }
    }

    public void IncrementKillCount()
    {
        PlaneKillCount++;
        Debug.Log($"Plane killed! Total kills: {PlaneKillCount}");
    }

    public void EndGame(bool isVictory)
    {
        isGameActive = false;

        if (isVictory)
        {
            Debug.Log("Victory! Loading Victory Scene...");
            SceneManager.LoadScene(victoryScene);
        }
        else
        {
            Debug.Log("Game Over! Loading Game Over Scene...");
            SceneManager.LoadScene(gameOverScene);
        }

        UnlockCursor(); // Unlock cursor when transitioning out of gameplay
    }

    public void PlayerDied()
    {
        EndGame(false);
    }

    public void RestartGame()
    {
        PlaneKillCount = 0; // Reset the kill count when restarting
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        LockCursor(); // Relock cursor when restarting gameplay
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorUnlocked = false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorUnlocked = true;
    }
}
