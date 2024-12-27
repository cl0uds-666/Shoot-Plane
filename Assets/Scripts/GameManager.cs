using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Lock the cursor to the game window and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Ensure the cursor stays locked if the game is active
        if (Cursor.lockState != CursorLockMode.Locked && Cursor.visible == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Handle Escape key to unlock and toggle the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops the play mode in the editor
#else
            Application.Quit(); // Closes the game
#endif
        }
    }
}
