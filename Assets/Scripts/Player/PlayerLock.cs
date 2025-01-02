using UnityEngine;

public class PlayerLock : MonoBehaviour
{
    [SerializeField] private Vector3 lockedPosition = new Vector3(0, 0, 0); // The player's fixed position
    [SerializeField] private Quaternion lockedRotation = Quaternion.identity; // The player's fixed rotation

    void Update()
    {
        // Lock the player's position and rotation
        transform.position = lockedPosition;
        transform.rotation = lockedRotation;
    }
}
