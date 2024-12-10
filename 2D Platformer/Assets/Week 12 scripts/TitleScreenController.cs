using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenController : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject instructionsPanel; 
    public AudioSource audioSource;
    public AudioClip clickSound; 

    void Start()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false); 
        }

        Debug.Log("Title Screen Loaded");
    }

    // Function to play the click sound
    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
    public void PlayGame()
    {
        PlayClickSound();
        Debug.Log("Play Game Button Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next scene
    }

    // Function to show instructions
    public void ShowInstructions()
    {
        PlayClickSound();
        Debug.Log("Instructions Button Clicked");
        if (mainMenuPanel != null && instructionsPanel != null)
        {
            mainMenuPanel.SetActive(false); // Hide main menu panel
            instructionsPanel.SetActive(true); // Show instructions panel
        }
    }

    // Function to hide instructions and return to the title screen
    public void BackToTitle()
    {
        PlayClickSound();
        Debug.Log("Back to Title Button Clicked");
        if (mainMenuPanel != null && instructionsPanel != null)
        {
            mainMenuPanel.SetActive(true); // Show main menu panel
            instructionsPanel.SetActive(false); // Hide instructions panel
        }
    }

    // Function to quit the game
    public void QuitGame()
    {
        PlayClickSound();
        Debug.Log("Quit Game Button Clicked");
        Application.Quit(); // Quit the application
#if UNITY_EDITOR
        Debug.Log("Quit function triggered (Editor Mode - Application will not close).");
#endif
    }
}
