using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class MainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip buttonClickClip;//sfx2, sfx3;

    public void PlayButtonSound()
    {
        PlayClip(buttonClickClip);
    }

    private void PlayClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    private IEnumerator WaitAndLoadScene(int LoadScene)
    {
        PlayButtonSound();

        // Wait for the sound to finish before loading the scene
        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene(LoadScene);
    }

    public void NewGame()
    {
        StartCoroutine(WaitAndLoadScene(1));
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        StartCoroutine(WaitAndLoadScene(1));
    }

    public void PauseGame()
    {
        PlayButtonSound();
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        PlayButtonSound();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitAndLoadScene(0));
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitAndExit());
    }

    private IEnumerator WaitAndExit()
    {
        PlayButtonSound();
        yield return new WaitForSeconds(0.4f);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}