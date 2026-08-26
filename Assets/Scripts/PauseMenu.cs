using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseCanvas;
    private bool isPaused = false;
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) Resume();
            else Pause();
        }
    }
    public void Pause()
    {
        isPaused = true;
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        isPaused = false;
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}