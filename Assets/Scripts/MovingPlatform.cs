using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Налаштування руху")]
    [Tooltip("Відстань руху вправо (локальна X)")]
    public float moveDistance = 20f;  // Відстань вправо-вліво

    [Tooltip("Швидкість (одиниці/сек)")]
    public float moveSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.localPosition = startPosition + new Vector3(t - moveDistance / 2f, 0f, 0f);
    }

    // Гравець їде разом з платформою
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.rigidbody;
            if (playerRb != null)
            {
                // Синхронізуємо позицію гравця з платформою
                playerRb.position += transform.position - transform.position;
            }
        }
    }
}
