using System;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    private BossController bossController;
    private int maxHealth;
    public int CurrentHealth { get; private set; }
    public bool IsInvincible { get; set; } = false;

    public event Action<int, int> OnHealthChanged;
    public event Action<Vector2, float> OnDamaged;
    public event Action OnDeath;

    private void Awake()
    {
        bossController = GetComponent<BossController>();
        CurrentHealth = bossController.BossHealth;
        maxHealth = bossController.BossHealth;
    }

    public void TakeDamage(int damage, Vector2 knockbackDir, float knockbackForce)
    {
        if (CurrentHealth <= 0 || IsInvincible) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        Debug.Log($"[BossHealth] Boss take {damage} dame! Curr HP: {CurrentHealth}/{maxHealth}");

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0)
        {
            Debug.Log("[BossHealth] Boss Dead!");
            OnDeath?.Invoke();
        }
        else
        {
            OnDamaged?.Invoke(knockbackDir, knockbackForce);
        }
    }
}