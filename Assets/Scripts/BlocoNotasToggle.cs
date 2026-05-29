using UnityEngine;

public class BlocoNotasToggle : MonoBehaviour
{
    public static BlocoNotasToggle Instance { get; private set; }

    public GameObject blocoNotasUI;
    public GameObject descobrirEntry;
    public GameObject bunnyEntry;
    public SafeInteraction safeInteraction;

    [Header("Notificação")]
    public GameObject notificacaoExclamacao;
    public AudioClip notificacaoSom;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            bool aberto = !blocoNotasUI.activeSelf;
            blocoNotasUI.SetActive(aberto);

            bool emZoom = safeInteraction != null && safeInteraction.IsZoomed;
            Cursor.lockState = (aberto || emZoom) ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = false;

            if (aberto && notificacaoExclamacao != null)
                notificacaoExclamacao.SetActive(false);
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

        if (notificacaoSom != null && SFXManager.Instance != null)
            SFXManager.Instance.PlaySFX(notificacaoSom);
    }
}
