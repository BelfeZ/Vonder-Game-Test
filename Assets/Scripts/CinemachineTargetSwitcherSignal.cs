using UnityEngine;
using Unity.Cinemachine;

public class CinemachineTargetSwitcherSignal : MonoBehaviour
{
    public Transform cameraTarget;
    public Transform playerTransform;
    public Transform itemTransform;

    public float smoothSpeed = 3f;

    private Transform desiredTarget;

    private void Start()
    {
        desiredTarget = playerTransform;

        if (cameraTarget != null && playerTransform != null)
            cameraTarget.position = playerTransform.position;
    }

    private void LateUpdate()
    {
        if (cameraTarget == null || desiredTarget == null) return;

        Vector3 targetPos = desiredTarget.position;
        targetPos.z = cameraTarget.position.z;

        cameraTarget.position = Vector3.Lerp(
            cameraTarget.position,
            targetPos,
            Time.deltaTime * smoothSpeed
        );
    }

    public void PanToTarget()
    {
        desiredTarget = itemTransform;
    }

    public void ReturnToPlayer()
    {
        desiredTarget = playerTransform;
    }
}