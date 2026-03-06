using UnityEngine;

public class Dart : MonoBehaviour
{
    [SerializeField] private float damage = 30f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        HealthManager health = other.GetComponentInParent<HealthManager>();
        if (health != null)
        {
            float before = health.currentEnergy;
            health.DecreaseEnergy(damage);
            float after = health.currentEnergy;
        }

        Destroy(gameObject);
    }
}
