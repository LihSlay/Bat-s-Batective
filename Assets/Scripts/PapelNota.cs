using UnityEngine;

// Marcador que se coloca em cada papel da Carruagem 4 (Papel1..Papel9). Ao olhar
// e premir E, ativa a respetiva entrada (pap1..pap9) no bloco de notas. É o
// PlayerInteraction quem o deteta.
public class PapelNota : MonoBehaviour
{
    [Tooltip("Entrada do bloco de notas que fica ativa ao interagir (ex.: pap1 para o Papel1).")]
    public GameObject notaEntry;

    // Verdadeiro enquanto a nota deste papel ainda não foi apontada.
    public bool NotaPorApontar => notaEntry != null && !notaEntry.activeSelf;

    // Ativa a entrada no bloco de notas. O MostrarPapel trata também da entrada
    // geral (papeisencontrar), que aparece na primeira interação com qualquer
    // papel, e só notifica (som + exclamação) se houver alguma entrada nova.
    public void MostrarNota()
    {
        if (notaEntry != null && BlocoNotasToggle.Instance != null)
            BlocoNotasToggle.Instance.MostrarPapel(notaEntry);
    }
}
