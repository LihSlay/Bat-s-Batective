using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public CanvasGroup dialogueCanvasGroup;
    public Button closeDialogueButton;
    public CanvasGroup optionsCanvasGroup;

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

        // Volta a mostrar diálogo
        if (ConversationManager.Instance.IsConversationActive)
        {
            dialogueCanvasGroup.alpha = 1f;

            closeDialogueButton.interactable = true;

            dialogueCanvasGroup.alpha = 1f;
            optionsCanvasGroup.alpha = 1f;
            optionsCanvasGroup.interactable = true;
            optionsCanvasGroup.blocksRaycasts = true;

            CanvasGroup buttonCanvas =
                closeDialogueButton.GetComponent<CanvasGroup>();

            buttonCanvas.alpha = 1f;
        }

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        // Esconde diálogo durante pause
        dialogueCanvasGroup.alpha = 0.2f;

        closeDialogueButton.interactable = false;

        dialogueCanvasGroup.alpha = 0.35f;
        optionsCanvasGroup.alpha = 0.35f;
        optionsCanvasGroup.interactable = false;
        optionsCanvasGroup.blocksRaycasts = false;

        CanvasGroup buttonCanvas =
            closeDialogueButton.GetComponent<CanvasGroup>();

        buttonCanvas.alpha = 0.6f;

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