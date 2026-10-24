using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    [Header("Reff Setting")]
    [SerializeField] private BossHealth bossHealth;
    [SerializeField] private BossController bossController;
    [SerializeField] private Image fillHPImage;
    [SerializeField] private Image decorateFillHPImage;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Smooth Effect")]
    [SerializeField] private float smoothSpeed = 5f;
    private float targetFill = 1f;

    [Header("Visibility Effect")]
    [SerializeField] private float fadeSpeed = 4f;
    private float targetAlpha = 0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        if (bossController == null)
        {
            GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");

            bossController = bossObj.GetComponent<BossController>();
            bossHealth = bossObj.GetComponent<BossHealth>();

        }
        else if (bossHealth == null)
        {
            bossHealth = bossController.GetComponent<BossHealth>();
        }
    }

    private void OnEnable() 
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged += UpdateHealthBar;
            bossHealth.OnDeath += HandleBossDeath;
        }
    }
    
    private void OnDisable()
    {
        if (bossHealth != null)
        {
            bossHealth.OnHealthChanged -= UpdateHealthBar;
            bossHealth.OnDeath -= HandleBossDeath;
        }
    }

    private void Update()
    {
        HandleFill();
        HandleVisibility();
    }

    private void HandleFill()
    {
        if (fillHPImage == null) return;

        fillHPImage.fillAmount = Mathf.Lerp(fillHPImage.fillAmount, 
                                            targetFill,
                                            smoothSpeed * Time.deltaTime);
    }

    private void HandleVisibility()
    {
        // Chặn lỗi null an toàn
        if (bossController == null || bossHealth == null || canvasGroup == null) return;

        // Kiểm tra xem Boss còn sống và Player có nằm trong tầm phát hiện không
        if (bossHealth.CurrentHealth > 0 && bossController.IsPlayerInDetectRange())
        {
            targetAlpha = 1f;
        }
        else 
        {
            targetAlpha = 0f;
        }

        // Tăng/giảm độ mờ mượt mà
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha,
                                              targetAlpha,
                                              fadeSpeed * Time.deltaTime);
    }

    private void UpdateHealthBar(int currHealth, int maxHealth)
    {
        if (maxHealth <= 0) return;
        targetFill = Mathf.Clamp01((float)currHealth / maxHealth);
    }

    private void HandleBossDeath()
    {
        targetFill = 0f;
        targetAlpha = 0f; 
    }
}