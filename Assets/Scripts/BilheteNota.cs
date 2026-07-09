using UnityEngine;

// Marcador que se coloca num bilhete (ex.: BilheteMesa, BilheteLixo, BilheteBanco).
// Ao olhar e premir E (por defeito só com a visão noturna ligada), ativa a
// respetiva entrada no bloco de notas. O PlayerInteraction é quem o deteta.
public class BilheteNota : MonoBehaviour
{
    [Tooltip("Objeto do bloco de notas que fica ativo ao interagir (ex.: Mesa, Lixo, Banco, ArmarioErrado).")]
    public GameObject notaEntry;

    [Tooltip("Opcional: entrada a mostrar quando o jogador está virado ao contrário " +
             "(ex.: ArmarioCerto). Se vazio, usa sempre a Nota Entry.")]
    public GameObject notaEntryContrario;

    [Tooltip("Se ligado, só interage com a visão noturna ativa.")]
    public bool soVisaoNoturna = true;

    [Tooltip("Se ligado, pode ser lido a qualquer distância quando o jogador está " +
             "virado ao contrário (ex.: BilheteArmario).")]
    public bool alcanceIlimitado = false;

    // A entrada certa consoante a orientação do jogador.
    private GameObject EntradaAtual =>
        (notaEntryContrario != null && PlayerController.IsUpsideDown)
            ? notaEntryContrario
            : notaEntry;

    // Verdadeiro se este bilhete pode ser interagido no estado atual.
    public bool PodeInteragir => !soVisaoNoturna || NightVision.IsNightVisionOn;

    // Verdadeiro enquanto a nota (a da orientação atual) ainda não foi apontada.
    public bool NotaPorApontar => EntradaAtual != null && !EntradaAtual.activeSelf;

    public void Interagir()
    {
        if (BlocoNotasToggle.Instance != null)
            BlocoNotasToggle.Instance.MostrarEntrada(EntradaAtual);
    }
}
