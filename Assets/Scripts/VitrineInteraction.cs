using UnityEngine;

// Marca a vitrine como interagível. Ao interagir com ela pela primeira vez,
// o PlayerInteraction ativa a entrada VitrineNota no bloco de notas.
public class VitrineInteraction : MonoBehaviour
{
    [Tooltip("Entrada VitrineNota que fica ativa na primeira interação com a vitrine.")]
    public GameObject vitrineNota;

    // Verdadeiro depois de a nota da vitrine já ter sido apontada.
    public bool NotaApontada => vitrineNota != null && vitrineNota.activeSelf;

    // Ativa a entrada no bloco de notas. MostrarEntrada só notifica (som +
    // exclamação) se a entrada ainda não estava ativa, por isso só acontece
    // na primeira interação.
    public void MostrarNota()
    {
        if (vitrineNota != null && BlocoNotasToggle.Instance != null)
            BlocoNotasToggle.Instance.MostrarEntrada(vitrineNota);
    }
}
