using UnityEngine;

public class Collector : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float value = 20f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Display display = Object.FindFirstObjectByType<Display>();
            HealthManager health = Object.FindFirstObjectByType<HealthManager>();

            if (gameObject.CompareTag("Coin"))
            {
                display.CollectCoin();
            }
            else if (gameObject.CompareTag("Energy"))
            {
                health.AddEnergy(value);
            }
            else if (gameObject.CompareTag("Life"))
            {
                health.AddLife();
            }

            Destroy(gameObject);
        }
    }
}