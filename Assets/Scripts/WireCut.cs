using UnityEngine;

public class WireCut : MonoBehaviour
{
    public GameObject fioCortado;

    public bool Cortado => cortado;

    private bool cortado = false;

    public InteractionUI mensagem;

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

            return;
        }

        Cut();
    }

    public void Cut()
    {
        if (cortado) return;

        cortado = true;

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