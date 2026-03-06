using UnityEngine;

public class EnemyBoard : MonoBehaviour
{
    [Header("Посилання")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject dartChild;

    [Header("Відстані")]
    [SerializeField] private float activationDistance = 25f;
    [SerializeField] private float deactivateDistance = 40f;

    [Header("Стрільба")]
    [SerializeField] private float fireRate = 1.7f;
    [SerializeField] private float dartSpeed = 22f;

    [Header("Поворот")]
    [SerializeField] private float rotationSpeed = 5f;

    private float nextFireTime;
    private bool isActive;

    void Awake()
    {
        if (dartChild == null)
            dartChild = transform.Find("Dart")?.gameObject;

        if (dartChild != null)
            dartChild.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isActive && distance <= activationDistance)
        {
            isActive = true;
            nextFireTime = Time.time + 0.6f;
        }
        else if (isActive && distance > deactivateDistance)
        {
            isActive = false;
            return;
        }

        if (!isActive) return;

        Vector3 direction = player.position - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(direction) *
                Quaternion.Euler(90f, 0f, 180f);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (dartChild == null) return;

        GameObject dart = Instantiate(dartChild, transform.position, transform.rotation);
        dart.SetActive(true);

        Rigidbody rb = dart.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 toPlayer = (player.position - transform.position).normalized;
            rb.linearVelocity = toPlayer * dartSpeed;
        }

        if (dart.GetComponent<Dart>() == null)
            dart.AddComponent<Dart>();

        Destroy(dart, 6f);
    }

    public void SetPlayer(Transform target)
    {
        player = target;
    }
}
