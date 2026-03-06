using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Display : MonoBehaviour
{
    [Header("Посилання")]
    [SerializeField] private PlayerMovement player;

    [Header("Панелі та текст")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text gameOverResultText;

    [Header("UI елементи")]
    [SerializeField] private Text speedText;
    [SerializeField] private Text coinsText;
    [SerializeField] private Slider energySlider;
    [SerializeField] private GameObject[] hearts;

    [Header("Ефекти втрати енергії")]
    [SerializeField] private Text energyLossText;
    [SerializeField] private float energyLossShowDuration = 0.8f;

    private HealthManager health;
    private Rigidbody playerRb;
    private int coinsCount;

    private void Awake()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerMovement>();
        }
    }

    private void Start()
    {
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody>();
        }

        health = GetComponent<HealthManager>();
        if (health == null)
        {
            health = FindFirstObjectByType<HealthManager>();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (energyLossText != null)
        {
            energyLossText.gameObject.SetActive(false);
        }

        UpdateUI();
    }

    private void Update()
    {
        if (playerRb != null && speedText != null)
        {
            float speed = new Vector3(playerRb.linearVelocity.x, 0f, playerRb.linearVelocity.z).magnitude;
            speedText.text = $"Speed: {speed:F2} m/s";
        }
    }

    public void CollectCoin()
    {
        coinsCount++;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (health == null) return;

        // Монети
        if (coinsText != null)
        {
            coinsText.text = $"Coin: {coinsCount}";
        }

        // Сердечка
        if (hearts != null)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                if (hearts[i] != null)
                {
                    hearts[i].SetActive(i < health.currentLives);
                }
            }
        }

        UpdateEnergySliderOnly();
    }

    public void UpdateEnergySliderOnly()
    {
        if (health == null || energySlider == null) return;

        energySlider.maxValue = health.maxEnergy;
        energySlider.value = health.currentEnergy;
    }

    public void ShowEnergyLoss(float amount)
    {
        if (energyLossText == null) return;

        energyLossText.text = $"-{amount:F0}";
        energyLossText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideEnergyLoss));
        Invoke(nameof(HideEnergyLoss), energyLossShowDuration);
    }

    private void HideEnergyLoss()
    {
        if (energyLossText != null)
        {
            energyLossText.gameObject.SetActive(false);
        }
    }

    public void GameOver(bool isWin)
    {
        if (gameOverPanel == null) return;

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        if (gameOverResultText != null)
        {
            gameOverResultText.text = isWin ? "ПЕРЕМОГА!" : "ГРА ЗАКІНЧЕНА";
        }
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}