using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target & Follow")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;

        if (Application.isPlaying)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, 
                                                    desiredPosition, 
                                                    smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
        else
        {
            transform.position = desiredPosition;
        }
    }
}