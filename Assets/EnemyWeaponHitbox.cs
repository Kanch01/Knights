using UnityEngine;

public class EnemyWeaponHitbox : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth h = other.GetComponent<PlayerHealth>();
            if (h != null)
            {
                h.TakeDamage(damage);
            }
        }
    }
}

