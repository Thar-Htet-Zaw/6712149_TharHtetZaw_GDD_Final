using UnityEngine;

public class DrivingPlayerController : MonoBehaviour
{
    public GameObject roads;                 // parent holding every road tile (drag Environment here)

    // Speed settings (units per second, and units per second squared)
    private float maxSpeed = 100.0f;          // top forward speed
    private float maxReverseSpeed = 100.0f;   // top backward speed
    private float acceleration = 8.0f;       // how fast the car speeds up while a key is held
    private float coastDeceleration = 5.0f;  // how fast it slows down after releasing the keys
    private float brakeDeceleration = 25.0f; // how fast it slows down when pressing the opposite key

    private float turnSpeed = 45.0f;
    private float horizontalInput;
    private float forwardInput;
    private float currentSpeed;              // signed: positive = forward, negative = backward

    private float edgeMargin = 2f;           // keeps the car body inside the road edge
    private Collider[] roadColliders;
    private Vector3 lastValidPos;
    private bool hasRoads;

    // Read-only access so the camera can tell when the car is really moving backward
    public float CurrentSpeed => currentSpeed;

    void Start()
    {
        if (roads == null)
        {
            Debug.LogWarning("PlayerController: assign the Roads object in the Inspector.");
            return;
        }

        // Remember every road tile separately (not one big box around all of them)
        roadColliders = roads.GetComponentsInChildren<Collider>();
        hasRoads = roadColliders.Length > 0;
        lastValidPos = transform.position;
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        // Raw input (-1, 0, 1) so Unity's built-in smoothing doesn't fight our own acceleration
        forwardInput = Input.GetAxisRaw("Vertical");

        // Works out the speed the car is trying to reach
        float targetSpeed = 0f;
        if (forwardInput > 0) targetSpeed = maxSpeed;
        else if (forwardInput < 0) targetSpeed = -maxReverseSpeed;

        // Picks how quickly the speed changes
        float rate;
        if (forwardInput == 0)
        {
            rate = coastDeceleration;        // no key held: roll to a stop
        }
        else if (currentSpeed != 0 && Mathf.Sign(forwardInput) != Mathf.Sign(currentSpeed))
        {
            rate = brakeDeceleration;        // opposite key held: brake hard
        }
        else
        {
            rate = acceleration;             // same direction: speed up gradually
        }
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.deltaTime);

        // Moves the car forward based on its current speed
        transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed);
        // Rotates the car based on horizontal input
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        // Keeps the car on the road, and stops it dead if it hits the edge
        if (hasRoads)
        {
            Vector3 pos = transform.position;
            if (!IsOnRoad(pos))
            {
                // Try sliding along the edge first, otherwise go back to the last safe spot
                Vector3 slideX = new Vector3(pos.x, pos.y, lastValidPos.z);
                Vector3 slideZ = new Vector3(lastValidPos.x, pos.y, pos.z);

                if (IsOnRoad(slideX)) pos = slideX;
                else if (IsOnRoad(slideZ)) pos = slideZ;
                else pos = new Vector3(lastValidPos.x, pos.y, lastValidPos.z);

                currentSpeed = 0f;
                transform.position = pos;
            }
            lastValidPos = transform.position;
        }
    }

    // True if the car, plus a margin on every side, is over the road
    bool IsOnRoad(Vector3 pos)
    {
        return PointOnRoad(pos.x + edgeMargin, pos.z)
            && PointOnRoad(pos.x - edgeMargin, pos.z)
            && PointOnRoad(pos.x, pos.z + edgeMargin)
            && PointOnRoad(pos.x, pos.z - edgeMargin);
    }

    // True if this X/Z spot lies inside ANY single road tile
    bool PointOnRoad(float x, float z)
    {
        foreach (Collider road in roadColliders)
        {
            Vector3 point = new Vector3(x, road.bounds.center.y, z);
            if ((road.ClosestPoint(point) - point).sqrMagnitude < 0.0001f) return true;
        }
        return false;
    }
}