using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public CanvasGroup dialogueCanvasGroup;
    public Button closeDialogueButton;
    public CanvasGroup optionsCanvasGroup;

    public SceneChanger sceneChanger;
    public SafeInteraction safeInteraction;

    private bool isPaused = false;
    public bool IsPaused => isPaused;

    // Acessível globalmente para bloquear inputs enquanto a pausa está aberta
    public static bool IsGamePaused { get; private set; }

    void Start()
    {
        // Garante que ao (re)carregar a cena do jogo o estado estático de pausa
        // está limpo — protege contra qualquer saída que não passe pelo Resume().
        isPaused = false;
        IsGamePaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isPaused)
            Pause();
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

        if (safeInteraction != null && safeInteraction.IsZoomed)
        {
            // Reaplica o cursor personalizado do zoom
            safeInteraction.ApplyZoomCursor();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Limpa a seleção para o botão "Continuar" não ficar realçado
        // da próxima vez que o menu de pausa abrir.
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        isPaused = false;
        IsGamePaused = false;
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

        // Garante que o menu abre sem nenhum botão pré-selecionado.
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        isPaused = true;
        IsGamePaused = true;
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

        // Sair para o menu não passa pelo Resume(), por isso repõe aqui o
        // estado de pausa. Sem isto, IsGamePaused (estático) fica preso em
        // true e bloqueia o virar/visão noturna ao recomeçar o jogo.
        isPaused = false;
        IsGamePaused = false;

        sceneChanger.LoadScene("MenuInicial");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        IsGamePaused = false;
        Application.Quit();
    }
}