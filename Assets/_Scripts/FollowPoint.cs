using UnityEngine;

public class FollowPoint : MonoBehaviour
{
    [SerializeField] private Vector3 rayDirection = Vector3.down;
    [SerializeField] private float rayDistance = 1f;
    [SerializeField] private float alignmentOffset = .15f;
    [SerializeField] LayerMask walkableLayer;
    private Transform _transform;

    void Awake()
    {
        _transform = transform;
    }
    void Update()
    {
        if (Physics.Raycast(_transform.position, rayDirection, out RaycastHit hitInfo, rayDistance, walkableLayer))
        {
            // If the ray hits something on the "Walkable" layer, set the position to the hit point
            _transform.position = hitInfo.point + Vector3.up * alignmentOffset;
            // Rotate the object to align with the surface normal
            Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            _transform.rotation = targetRotation;


        }
    }
}
