using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isCursorUnlocked = false; // Track whether the cursor is unlocked

    void Start()
    {
        // Lock the cursor to the game window and make it invisible
        LockCursor();
    }

    void Update()
    {
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
