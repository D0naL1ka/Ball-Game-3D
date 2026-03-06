using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Рух")]
    public float moveSpeed = 8f;

    [Header("Обертання м'яча")]
    public float rotationSpeed = 8f;

    [Header("Стрибок")]
    public float jumpForce = 10f;
    public float groundCheckDistance = 0.6f;
    public LayerMask groundMask = -1;

    [Header("Поворот на 90°")]
    public float doubleTapWindow = 0.25f;
    public float rotationSmoothness = 12f;

    [Header("Падіння")]
    public float fallThreshold = -10f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    [HideInInspector] public Quaternion stableRotation;

    private Rigidbody rb;
    private bool jumpRequested = false;

    private float lastTapTimeD = -1f;
    private float lastTapTimeA = -1f;

    private bool isSnapping = false;
    private Quaternion snapTargetRotation;

    private Display display;
    private HealthManager health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Дозволяємо обертання для котіння
        rb.constraints = RigidbodyConstraints.FreezeRotationY;

        startPosition = transform.position;
        startRotation = transform.rotation;
        stableRotation = startRotation;

        display = FindFirstObjectByType<Display>();
        health = FindFirstObjectByType<HealthManager>();
    }

    void Update()
    {
        CheckDoubleTap(KeyCode.D, ref lastTapTimeD, 90f);
        CheckDoubleTap(KeyCode.A, ref lastTapTimeA, -90f);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpRequested = true;
        }

        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    void FixedUpdate()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 forward = stableRotation * Vector3.forward;
        Vector3 right = stableRotation * Vector3.right;

        Vector3 moveDirection = (forward * inputZ + right * inputX).normalized;

        // Рух
        rb.linearVelocity = new Vector3(
            moveDirection.x * moveSpeed,
            rb.linearVelocity.y,
            moveDirection.z * moveSpeed
        );

        // Обертання м'яча
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDirection);
            rb.angularVelocity = rotationAxis * rotationSpeed;
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }

        // Плавний поворот на 90°
        if (isSnapping)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                snapTargetRotation,
                rotationSmoothness * Time.fixedDeltaTime
            );

            if (Quaternion.Angle(transform.rotation, snapTargetRotation) < 0.1f)
            {
                transform.rotation = snapTargetRotation;
                stableRotation = snapTargetRotation;
                isSnapping = false;
            }
        }

        // Стрибок
        if (jumpRequested && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }
    }

    // Respawn
    public void Respawn()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.rotation = startRotation;
        stableRotation = startRotation;
        isSnapping = false;

        if (health != null)
        {
            health.TakeDamage();
        }
    }

    // Подвійне натискання
    private void CheckDoubleTap(KeyCode key, ref float lastTime, float angle)
    {
        if (Input.GetKeyDown(key))
        {
            if (Time.time - lastTime < doubleTapWindow)
            {
                snapTargetRotation = stableRotation * Quaternion.Euler(0, angle, 0);
                isSnapping = true;
                lastTime = -1f;
            }
            else
            {
                lastTime = Time.time;
            }
        }
    }

    // Перевірка землі
    private bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            groundCheckDistance,
            groundMask
        );
    }

    // Фініш
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("End"))
        {
            if (health != null && display != null &&
                health.currentEnergy > 0 &&
                health.currentLives > 0)
            {
                display.GameOver(true);
            }
        }
    }
}
