using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public AudioSource somVidro;

    private bool partido = false;
    public InteractionUI mensagem;

    public void BreakGlass()
    {
        if (partido)
            return;

        if (!MarteloPickup.HasMartelo)
        {
            if (mensagem != null)
            {
                mensagem.FadeIn();
                Invoke(nameof(EsconderMensagem), 2f);
            }

            return;
        }

        partido = true;

        if (somVidro != null)
            somVidro.Play();

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    void EsconderMensagem()
    {
        if (mensagem != null)
            mensagem.FadeOut();
    }
}