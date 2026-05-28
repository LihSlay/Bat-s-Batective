using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public SceneChanger sceneChanger;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void ShowPauseMenu()
    {
        pauseMenuUI.SetActive(true);
    }

    public void GoToSettings()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Definições", LoadSceneMode.Additive);
    }

        public void GoToControls()
    {
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(false);
        SceneManager.LoadScene("Controlos", LoadSceneMode.Additive);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        sceneChanger.LoadScene("MenuInicial");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
