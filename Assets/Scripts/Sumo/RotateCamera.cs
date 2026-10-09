using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotationSpeed = 50f;

    void Update()
    {
        // Rotates the focal point (and the camera attached to it) around the island
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}