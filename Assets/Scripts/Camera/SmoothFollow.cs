using UnityEngine;

[ExecuteInEditMode]
public class SmoothFollow : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    public Quaternion rotationalOffset = Quaternion.identity;
    public float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    private Vector3 targetPosition;

    public void LateRefresh()
    {
        FollowTarget(target.position + (target.rotation * offset));
    }


    private void FollowTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        transform.LookAt(target, target.up);
        transform.rotation *= rotationalOffset;
    }

    public void OnPositionChange(Vector3 newPosition)
    {
        transform.position -= newPosition;
    }
}
