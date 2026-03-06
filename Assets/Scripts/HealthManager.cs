using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [Header("Параметри")]
    [SerializeField] public float maxEnergy = 100f;
    [SerializeField] private int maxLives = 5;

    [Header("Поточні значення")]
    public float currentEnergy;
    public int currentLives;

    private Display display;

    void Awake()
    {
        currentEnergy = maxEnergy;
        currentLives = maxLives;

        display = GetComponent<Display>();
        if (display == null)
        {
            display = FindFirstObjectByType<Display>();
        }
    }

    public void DecreaseEnergy(float amount)
    {
        if (currentEnergy <= 0) return;

        currentEnergy -= amount;

        if (display != null)
        {
            display.ShowEnergyLoss(amount);
            display.UpdateEnergySliderOnly();
        }

        // Якщо енергія закінчилася — втрата життя + респавн
        if (currentEnergy <= 0)
        {
            currentLives--;

            if (display != null)
            {
                display.UpdateUI();
            }

            if (currentLives <= 0)
            {
                if (display != null)
                {
                    display.GameOver(false);
                }
                return; // кінець
            }

            // енергія після втрати життя
            currentEnergy = maxEnergy;

            if (display != null)
            {
                display.UpdateEnergySliderOnly();
            }

            // Респавн
            PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                player.Respawn();
            }
        }
    }

    public void AddEnergy(float amount)
    {
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy + amount);
        if (display != null)
        {
            display.UpdateEnergySliderOnly();
        }
    }

    public void AddLife()
    {
        if (currentLives < maxLives)
        {
            currentLives++;
            if (display != null)
            {
                display.UpdateUI();
            }
        }
    }

    public void TakeDamage()
    {
        currentLives--;

        if (display != null)
        {
            display.UpdateUI();
        }

        if (currentLives <= 0)
        {
            if (display != null)
            {
                display.GameOver(false);
            }
        }
    }
}