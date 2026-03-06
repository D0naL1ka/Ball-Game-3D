using UnityEngine;

public class HammerRotator : MonoBehaviour
{
    [Header("Налаштування обертання")]
    [Tooltip("Центр, навколо якого обертатися (перетягни порожній об’єкт)")]
    public Transform rotationCenter;

    [Tooltip("Швидкість обертання (градуси за секунду)")]
    public float rotationSpeed = 90f;

    [Tooltip("За годинниковою стрілкою?")]
    public bool clockwise = true;

    void Update()
    {
        if (rotationCenter == null)
        {
            Debug.LogWarning("Rotation center не призначено!");
            return;
        }

        // Обертання навколо вказаного центру
        float direction = clockwise ? -1f : 1f;
        transform.RotateAround(rotationCenter.position, Vector3.up, direction * rotationSpeed * Time.deltaTime);
    }
}
