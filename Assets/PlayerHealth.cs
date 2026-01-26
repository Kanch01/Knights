using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;
    
    public SpriteRenderer spriteRenderer;
    public Color hurtColor = Color.firebrick;
    public float flashDuration = 0.15f;
    
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
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateLives(currentHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateLives(currentHealth);
        }
        
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPlayerHurt();
        }
        
        if (spriteRenderer != null)
        {
            if (flashRoutine != null)
                StopCoroutine(flashRoutine);

            flashRoutine = StartCoroutine(HurtFlash());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private IEnumerator HurtFlash()
    {
        spriteRenderer.color = hurtColor;

        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float lerp = t / flashDuration;
            spriteRenderer.color = Color.Lerp(hurtColor, originalColor, lerp);
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}
