using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    public AudioSource musicSource;
    public AudioSource sfxSource;
    
    public AudioClip themeMusic;
    public AudioClip winMusic;
    
    public AudioClip playerAttackClip;
    public AudioClip playerHurtClip;
    public AudioClip enemyDeathClip;
    public AudioClip enemyAttackClip;
    public AudioClip dashClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayThemeMusic();
    }

    public void PlayThemeMusic()
    {
        if (musicSource == null || themeMusic == null) return;

        musicSource.Stop();
        musicSource.clip = themeMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayWinMusic()
    {
        if (musicSource == null || winMusic == null) return;

        musicSource.Stop();
        musicSource.clip = winMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayPlayerAttack()
    {
        PlaySFX(playerAttackClip);
    }

    public void PlayPlayerHurt()
    {
        PlaySFX(playerHurtClip);
    }

    public void PlayEnemyDeath()
    {
        PlaySFX(enemyDeathClip);
    }

    public void PlayEnemyAttack()
    {
        PlaySFX(enemyAttackClip);
    }

    public void PlayDash()
    {
        PlaySFX(dashClip);
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip);
    }
}

