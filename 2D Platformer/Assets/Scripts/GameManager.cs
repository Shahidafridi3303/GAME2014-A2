using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } 

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI coinsText; 

    private int totalCoins = 0; 

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