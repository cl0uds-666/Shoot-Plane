using UnityEngine;

public class MiniMapIconRotation : MonoBehaviour
{
    [SerializeField] private Quaternion fixedRotation = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f);
    [SerializeField] private Vector3 fixedEulerAngles = new Vector3(90f, 90f, 0f);

    private void LateUpdate()
    {
        // Uncomment the method you want to use, either Quaternion or Euler angles

        // Option 1: Use fixed Quaternion rotation
        transform.rotation = fixedRotation;

        // Option 2: Use fixed Euler angles
        // transform.rotation = Quaternion.Euler(fixedEulerAngles);
    }
}
