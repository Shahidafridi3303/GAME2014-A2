using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip enemyAttackSound;
    [SerializeField] private AudioClip IceWallAbilitySound;
    [SerializeField] private AudioClip BlockAbilitySound;
    [SerializeField] private AudioClip playerJumpSound;
    [SerializeField] private AudioClip playerHurtSound;
    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip buttonClickSound;

    private AudioSource audioSource;

    void Awake()
    {
        // Ensure there's only one instance of SoundManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep SoundManager persistent
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Project-Specific Sound Playback Methods
    public void PlayCoinPickupSound() => PlaySound(coinPickupSound);
    public void PlayEnemyAttackSound() => PlaySound(enemyAttackSound);
    public void PlayIceWallAbilitySound() => PlaySound(IceWallAbilitySound);
    public void PlayBlockAbilitySound() => PlaySound(BlockAbilitySound);
    public void PlayPlayerJumpSound() => PlaySound(playerJumpSound);
    public void PlayPlayerHurtSound() => PlaySound(playerHurtSound);
    public void PlayEnemyDeathSound() => PlaySound(enemyDeathSound);
    public void PlayPlayerDeathSound() => PlaySound(playerDeathSound);
    public void PlayButtonClickSound() => PlaySound(buttonClickSound);
}