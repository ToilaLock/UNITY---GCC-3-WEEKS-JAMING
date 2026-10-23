using UnityEngine;

public class DamageSender : MonoBehaviour
{
    [Header("Damage Setting")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float knockBackForce = 8f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponentInParent<IDamageable>();

        if(target == null) return;
        Vector2 knockBackDirection = (collision.transform.position - collision.transform.root.position).normalized;
        target.TakeDamage(damage, knockBackDirection, knockBackForce);
    }
}
