using UnityEngine;

public class GemRotation : MonoBehaviour {

    public float xForce = 0, yForce = 0, zForce = 0, speedMultiplier = 1;

    [Header("Hover")]
    public Transform target;
    public Vector3 velocity = Vector3.zero;
    public float speed;

    [Header("Debug")]
    public Vector3 initPosition;
    public bool goingUp = true;


    private void Start()
    {
        initPosition = transform.position;
    }
    void Update () {
        this.transform.Rotate (xForce * speedMultiplier, yForce * speedMultiplier, zForce * speedMultiplier);
        if (goingUp)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, speed);
            if (transform.position == target.position)
            {
                goingUp = false;
            }
        } else
        {
            transform.position = Vector3.SmoothDamp(transform.position, initPosition, ref velocity, speed);
            if (transform.position == initPosition)
            {
                goingUp = true;
            }
        }
    }
    public void UpdateRotationSpeed(float newSpeed)
    {
        speedMultiplier = newSpeed;
    }

}