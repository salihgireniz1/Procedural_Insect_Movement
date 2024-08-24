using UnityEngine;

public class BodyController : MonoBehaviour
{
    [Header("Body Rotation Related To Legs")]
    [SerializeField] private Transform bodyTransform;
    [SerializeField] ProceduralLegTarget[] legs;
    [SerializeField] private float smoothTime = 0.3f; // Time it takes to smooth the movement
    [SerializeField] private float yOffset;
    [SerializeField] private float rotationLerpSpeed = 5f;


    [Header("Spider Mover")]
    [SerializeField] private float moveSpeed = 5f; // Speed of forward/backward movement
    [SerializeField] private float rotateSpeed = 100f; // Speed of rotation
    private float _currentVelocityY;

    private void Update()
    {
        // Handle WASD input for movement and rotation
        HandleMovement();

        // Calculate the average position and move the body
        Vector3 averagePosition = CalculateAverageLegPosition();
        float smoothY = Mathf.SmoothDamp(bodyTransform.position.y, averagePosition.y + yOffset, ref _currentVelocityY, smoothTime);
        bodyTransform.position = new Vector3(averagePosition.x, smoothY, averagePosition.z);

        // Calculate and apply the rotation based on the leg positions
        Quaternion targetRotation = CalculateBodyRotation();
        bodyTransform.rotation = Quaternion.Slerp(bodyTransform.rotation, targetRotation, Time.deltaTime * rotationLerpSpeed);
    }
    private void HandleMovement()
    {
        // Get input from the user
        float moveInput = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys
        float rotateInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys

        // Move the transform forward/backward
        transform.Translate(Vector3.forward * moveInput * moveSpeed * Time.deltaTime);

        // Rotate the transform left/right
        transform.Rotate(Vector3.up, rotateInput * rotateSpeed * Time.deltaTime);
    }
    private Vector3 CalculateAverageLegPosition()
    {
        Vector3 sumPositions = Vector3.zero;
        int totalLegs = legs.Length;

        foreach (var leg in legs)
        {
            sumPositions += leg.transform.position;
        }

        return sumPositions / totalLegs;
    }
    private Quaternion CalculateBodyRotation()
    {
        if (legs.Length < 3) return bodyTransform.rotation; // Need at least 3 points to define a plane

        // Choose three legs to define the plane
        Vector3 leg1 = legs[0].transform.position;
        Vector3 leg2 = legs[1].transform.position;
        Vector3 leg3 = legs[2].transform.position;

        // Define two vectors on the plane
        Vector3 v1 = leg2 - leg1;
        Vector3 v2 = leg3 - leg1;

        // Calculate the normal of the plane
        Vector3 normal = Vector3.Cross(v1, v2).normalized;

        // Calculate the rotation that aligns the body's up direction with the plane's normal
        Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, normal);

        // Only keep the X and Z rotations
        Vector3 euler = targetRotation.eulerAngles;
        targetRotation = Quaternion.Euler(euler.x, bodyTransform.rotation.eulerAngles.y, euler.z);

        return targetRotation;
    }
}
