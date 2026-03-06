using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Налаштування камери")]
    public PlayerMovement playerScript;
    public Vector3 offset = new Vector3(0, 3.5f, -7f);
    public float smoothSpeed = 0.12f;               // Швидкість слідування
    public float rotationSmoothSpeed = 5f;          // Швидкість обертання камери

    void LateUpdate()
    {
        if (playerScript == null) return;

        Vector3 rotatedOffset = playerScript.stableRotation * offset;
        Vector3 desiredPosition = playerScript.transform.position + rotatedOffset;

        // Плавне переміщення позиції
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Камера завжди дивиться на гравця
        Vector3 lookDirection = playerScript.stableRotation * Vector3.forward;
        Vector3 lookTarget = playerScript.transform.position + lookDirection * 2f + Vector3.up * 1.5f;

        Quaternion desiredRotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}