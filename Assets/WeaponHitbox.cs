using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Health h = other.GetComponent<Health>();
            if (h != null)
            {
                h.TakeDamage(damage);
            }
        }
    }
}

