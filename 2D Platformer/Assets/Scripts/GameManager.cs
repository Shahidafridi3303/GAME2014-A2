using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } 

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinsText; 

    private int totalCoins = 0;

    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // called third
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        coinsText = GameObject.Find("Score UI").GetComponent<TextMeshProUGUI>();
        coinsText.text = $"Coins: {totalCoins}";
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Function to add coins and update the UI
    public void AddCoins(int amount)
    {
        SoundManager.instance.PlayCoinPickupSound();

        totalCoins += amount;
        UpdateCoinsUI();
    }

    // Function to update the coins UI
    private void UpdateCoinsUI()
    {
        if (coinsText != null)
        {
            coinsText.text = $"Coins: {totalCoins}";
        }
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }
}