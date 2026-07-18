using UnityEngine;

public class GlassBreak : MonoBehaviour
{
    public AudioSource somVidro;

    private bool partido = false;

    public void BreakGlass()
    {
        if (partido)
            return;

        partido = true;

        if (somVidro != null)
            somVidro.Play();

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}