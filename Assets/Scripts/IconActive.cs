using UnityEngine;
using UnityEngine.UI;

public class IconActive : MonoBehaviour
{
    public static IconActive Instance { get; private set; }

    [Header("Bloco de Notas")]
    [SerializeField] private Image imagemBloco;
    [SerializeField] private Sprite blocoNormal;
    [SerializeField] private Sprite blocoAtivo;

    [Header("Visão Noturna")]
    [SerializeField] private Image imagemVisao;
    [SerializeField] private Sprite visaoNormal;
    [SerializeField] private Sprite visaoAtivo;

    [Header("Flip")]
    [SerializeField] private Image imagemFlip;
    [SerializeField] private Sprite flipNormal;
    [SerializeField] private Sprite flipAtivo;

    [Header("Ouvir")]
    [SerializeField] private Image imagemOuvir;
    [SerializeField] private Sprite ouvirNormal;
    [SerializeField] private Sprite ouvirAtivo;

    private bool visaoLigada;
    private bool ouvirLigado;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (PauseMenu.IsGamePaused) return;

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            visaoLigada = !visaoLigada;
            if (imagemVisao != null)
                imagemVisao.sprite = visaoLigada ? visaoAtivo : visaoNormal;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ouvirLigado = !ouvirLigado;
            if (imagemOuvir != null)
                imagemOuvir.sprite = ouvirLigado ? ouvirAtivo : ouvirNormal;
        }

    }

    public void SetBlocoAtivo(bool ativo)
    {
        if (imagemBloco != null)
            imagemBloco.sprite = ativo ? blocoAtivo : blocoNormal;
    }

    public void SetFlipAtivo(bool ativo)
    {
        if (imagemFlip != null)
            imagemFlip.sprite = ativo ? flipAtivo : flipNormal;
    }
}
