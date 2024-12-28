using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90.0f; // Speed for horizontal rotation

    void Update()
    {
        // Use mouse X-axis for horizontal rotation
        float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, horizontalRotation, 0, Space.Self);
    }
}
