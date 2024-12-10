using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{
    [SerializeField] private GameObject EndgameScreen;
    [SerializeField] private AudioClip endgameMusic;
    [SerializeField] private TextMeshProUGUI scoreText;
    private AudioSource audioSource;
    private GameManager gameManager;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = endgameMusic;
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                EndgameScreen.SetActive(true);
                audioSource.Play();
                scoreText.text = $"Score: {gameManager.TotalCoins}";
            }
        }
    }
}