using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerController playerController;

    [Header("UI Fill")]
    [SerializeField] private Image healthFillImage;
    [SerializeField] private Image staminaFillImage;

    [Header("Smoothing")]
    [SerializeField] private float smoothSpeed = 8f;

    private float targetHealthFill = 1f;
    private float targetStaminaFill = 1f;

    private void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealth;
            UpdateHealth(playerHealth.CurrentHealth, playerHealth.MaxHealth);
        }

        if (playerController != null)
        {
            playerController.OnStaminaChanged += UpdateStamina;
            UpdateStamina(playerController.CurrentStamina, playerController.MaxStamina);
        }
    }

    private void OnDestroy()
    {
        playerHealth.OnHealthChanged -= UpdateHealth;
        playerController.OnStaminaChanged -= UpdateStamina;
    }

    private void Update()
    {
        healthFillImage.fillAmount = Mathf.Lerp(healthFillImage.fillAmount, 
                                                targetHealthFill, 
                                                Time.unscaledDeltaTime * smoothSpeed);

        staminaFillImage.fillAmount = Mathf.Lerp(staminaFillImage.fillAmount, 
                                                targetStaminaFill, 
                                                Time.unscaledDeltaTime * smoothSpeed);
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (maxHealth <= 0) return;
        targetHealthFill = Mathf.Clamp01((float)currentHealth / maxHealth);
    }

    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        if (maxStamina <= 0) return;
        targetStaminaFill = Mathf.Clamp01(currentStamina / maxStamina);
    }
}