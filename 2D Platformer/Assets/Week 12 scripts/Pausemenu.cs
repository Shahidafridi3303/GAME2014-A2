using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Pause menu panel
    public GameObject gameOverPanel; // GameOver screen panel
    public GameObject defaultUIPanel; // Default in-game UI panel
    public AudioSource clickSoundSource; // AudioSource for click sound effects
    public AudioClip clickSound; // Click sound effect

    private bool isPaused = false;

    void Start()
    {
        // Ensure proper panel visibility at the start
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (defaultUIPanel != null) defaultUIPanel.SetActive(true);
    }

    // Function to pause the game
    public void PauseGame()
    {
        PlayClickSound();
        if (pauseMenuPanel != null && defaultUIPanel != null)
        {
            isPaused = true;
            Time.timeScale = 0; // Pause the game
            pauseMenuPanel.SetActive(true); // Show Pause Menu Panel
            defaultUIPanel.SetActive(false); // Hide Default UI
        }
    }

    // Function to resume the game
    public void ResumeGame()
    {
        PlayClickSound();
        if (pauseMenuPanel != null && defaultUIPanel != null)
        {
            isPaused = false;
            Time.timeScale = 1; // Resume the game
            pauseMenuPanel.SetActive(false); // Hide Pause Menu Panel
            defaultUIPanel.SetActive(true); // Show Default UI
        }
    }

    // Function to load the next scene
    public void NextScene()
    {
        PlayClickSound();
        Time.timeScale = 1; // Ensure game time is resumed before loading the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene
    }

    // Function to show the GameOver screen
    public void GameOver()
    {
        PlayClickSound();
        if (gameOverPanel != null)
        {
            Time.timeScale = 0; // Pause the game for GameOver
            gameOverPanel.SetActive(true); // Show GameOver Panel
            if (defaultUIPanel != null) defaultUIPanel.SetActive(false); // Hide Default UI
            if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false); // Hide Pause Menu Panel
        }
    }

    // Function to hide the GameOver UI and restore default UI
    public void HideUI()
    {
        PlayClickSound();
        if (gameOverPanel != null && defaultUIPanel != null)
        {
            Time.timeScale = 1; // Resume the game
            gameOverPanel.SetActive(false); // Hide GameOver Panel
            defaultUIPanel.SetActive(true); // Show Default UI
        }
    }

    // Function to play click sound
    private void PlayClickSound()
    {
        if (clickSoundSource != null && clickSound != null)
        {
            clickSoundSource.PlayOneShot(clickSound);
        }
    }
}
