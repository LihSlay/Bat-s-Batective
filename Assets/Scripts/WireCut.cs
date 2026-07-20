using UnityEngine;

public class WireCut : MonoBehaviour
{
    public GameObject fioCortado;

    public AudioClip somCorte;

    public bool Cortado => cortado;

    private bool cortado = false;

    public InteractionUI mensagem;

    [Header("Bloco de Notas")]
    [Tooltip("Entrada FiosNota que fica ativa na primeira vez que o jogador tenta cortar um fio sem o alicate.")]
    public GameObject fiosNota;

    private void OnMouseDown()
    {
        // Só deixa cortar se o jogador tiver o alicate na mão
        if (!AlicatePickup.HasAlicate)
        {
            if (mensagem != null)
            {
                mensagem.FadeIn();
                Invoke(nameof(EsconderMensagem), 2f);
            }

            // Primeira vez que tenta cortar sem alicate: ativa a nota no bloco.
            // MostrarEntrada só notifica se a entrada ainda não estava ativa.
            if (fiosNota != null && BlocoNotasToggle.Instance != null)
                BlocoNotasToggle.Instance.MostrarEntrada(fiosNota);

            return;
        }

        Cut();
    }

    public void Cut()
    {
        if (cortado) return;

        cortado = true;
        if (somCorte != null && SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySFX(somCorte);
        }

        gameObject.SetActive(false);
        fioCortado.SetActive(true);

        Debug.Log("Fio cortado!");
    }

    void EsconderMensagem()
    {
        if (mensagem != null)
            mensagem.FadeOut();
    }
}