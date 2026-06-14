using UnityEngine;

public class BlocoNotasToggle : MonoBehaviour
{
    public static BlocoNotasToggle Instance { get; private set; }

    public GameObject blocoNotasUI;
    public GameObject descobrirEntry;
    public GameObject bunnyEntry;
    public SafeInteraction safeInteraction;
    public PauseMenu pauseMenu;

    [Header("Notificação")]
    public GameObject notificacaoExclamacao;
    public AudioClip notificacaoSom;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Não permite abrir/fechar o bloco de notas com o menu de pausa aberto
        if (pauseMenu != null && pauseMenu.IsPaused) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool aberto = !blocoNotasUI.activeSelf;
            blocoNotasUI.SetActive(aberto);

            bool emZoom = safeInteraction != null && safeInteraction.IsZoomed;

            if (!aberto && emZoom)
            {
                // Ao fechar o bloco dentro do zoom, repõe o cursor personalizado visível
                safeInteraction.ApplyZoomCursor();
            }
            else
            {
                Cursor.lockState = (aberto || emZoom) ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (aberto && notificacaoExclamacao != null)
                notificacaoExclamacao.SetActive(false);

            if (aberto)
                IconActive.Instance?.SetBlocoAtivo(false);
        }
    }

    public void MostrarEntradaCofre()
    {
        if (descobrirEntry != null)
            descobrirEntry.SetActive(true);
        MostrarNotificacao();
    }

    public void MostrarEntradaBunny()
    {
        if (bunnyEntry != null)
            bunnyEntry.SetActive(true);
        MostrarNotificacao();
    }

    private void MostrarNotificacao()
    {
        if (notificacaoExclamacao != null)
            notificacaoExclamacao.SetActive(true);

        IconActive.Instance?.SetBlocoAtivo(true);

        if (notificacaoSom != null && SFXManager.Instance != null)
            SFXManager.Instance.PlaySFX(notificacaoSom);
    }
}
