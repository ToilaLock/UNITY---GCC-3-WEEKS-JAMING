using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerController playerController;
    public int MaxHealth => playerController.HealthPoint;

    public int CurrentHealth { get; private set; }
    public bool IsInvincible { get; set; } = false;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;
    public event Action<Vector2, float> OnDamaged;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage, Vector2 knockBackDirection, float knockBackForce)
    {
        if (CurrentHealth <= 0 || IsInvincible) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        Debug.Log($"HP: {CurrentHealth}");

        if(CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
        else
        {
            OnDamaged?.Invoke(knockBackDirection, knockBackForce);
        }
    }

    [ContextMenu("Debug: Take 20 Damage")]
    private void DebugTakeDamage()
    {
        TakeDamage(20, Vector2.left, 8f);
    }
}