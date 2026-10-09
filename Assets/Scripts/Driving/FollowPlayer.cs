using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private DrivingPlayerController playerController;
    private Vector3 offset = new Vector3(0, 5, -7);
    private float pitch = 18f;         // downward tilt of the camera
    private float viewAngle = 0f;      // 0 = behind the car, 180 = in front of the car
    private float swingSpeed = 360f;   // degrees per second when switching views

    void Start()
    {
        playerController = player.GetComponent<DrivingPlayerController>();
    }

    // LateUpdate runs after all Update calls, which removes camera jitter
    void LateUpdate()
    {
        // The camera goes to the front only when the car is actually rolling backward
        bool reversing = playerController.CurrentSpeed < -0.1f;
        float targetAngle = reversing ? 180f : 0f;
        viewAngle = Mathf.MoveTowardsAngle(viewAngle, targetAngle, swingSpeed * Time.deltaTime);

        // Follow the car's turning (yaw only, so a tipping car doesn't tilt the camera)
        Quaternion yaw = Quaternion.Euler(0, player.transform.eulerAngles.y + viewAngle, 0);

        transform.position = player.transform.position + yaw * offset;
        transform.rotation = yaw * Quaternion.Euler(pitch, 0, 0);
    }
}