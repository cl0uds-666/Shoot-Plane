using UnityEngine;
using TMPro; // For TextMeshPro
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float survivalTime = 120f; // Time to survive in seconds
    [SerializeField] private TextMeshProUGUI timerText; // TextMeshPro timer display
    [SerializeField] private string gameOverScene = "GameOver"; // Name of the Game Over scene
    [SerializeField] private string victoryScene = "Victory"; // Name of the Victory scene

    private bool isGameActive = true;
    private float remainingTime;
    private bool isCursorUnlocked = false; // Track whether the cursor is unlocked

    void Start()
    {
        remainingTime = survivalTime;
        UpdateTimerUI();

        // Lock the cursor to the game window and make it invisible
        LockCursor();
    }

    void Update()
    {
        if (isGameActive)
        {
            HandleSurvivalTimer();
        }

        // Toggle cursor lock/unlock with the Escape key
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

    private void HandleSurvivalTimer()
    {
        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            EndGame(true); // Player survived
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void EndGame(bool isVictory)
    {
        isGameActive = false;

        if (isVictory)
        {
            Debug.Log("Victory! Loading Victory Scene...");
            SceneManager.LoadScene("Win");
        }
        else
        {
            Debug.Log("Game Over! Loading Game Over Scene...");
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void PlayerDied()
    {
        EndGame(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
