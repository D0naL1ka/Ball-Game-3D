using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Header("Налаштування сили")]
    public float upwardForce = 15f;   // Сила стрибка вгору
    public float forwardForce = 20f;  // Сила поштовху вперед

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // новий вектор швидкості
                Vector3 launchVelocity = (Vector3.up * upwardForce) + (transform.forward * forwardForce);

                // швидкість напряму - змушує фізичний рушій ігнорувати попередній стан м'яча
                rb.linearVelocity = launchVelocity;
            }
        }
    }
}