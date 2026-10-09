using UnityEngine;

public class SpinPropellerX : MonoBehaviour
{
    public float spinSpeed = 500f;

    void Update()
    {
        transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
    }
}