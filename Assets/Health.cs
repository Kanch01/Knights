using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int maxHealth = 1;
    private int currentHealth;
    
    public SpriteRenderer spriteRenderer;

    public Color hurtColor = Color.firebrick;
    public float hurtFlashDuration = 0.15f;

    private Color originalColor;
    private Coroutine flashRoutine;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }
    
    public void InitializeHealth(int newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (spriteRenderer != null)
            {
                if (flashRoutine != null)
                    StopCoroutine(flashRoutine);

                flashRoutine = StartCoroutine(HurtFlash());
            }
        }
    }
    
    private IEnumerator HurtFlash()
    {
        spriteRenderer.color = hurtColor;

        float t = 0f;
        while (t < hurtFlashDuration)
        {
            t += Time.deltaTime;
            float lerp = t / hurtFlashDuration;
            spriteRenderer.color = Color.Lerp(hurtColor, originalColor, lerp);
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyKilled();
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyDeath();
        }
        
        Destroy(gameObject);
    }
}

