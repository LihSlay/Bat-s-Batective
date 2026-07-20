using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    private Animator animator;
    public AudioClip somClique;

    public bool ligado;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Ligado", ligado);
    }

    public void Alternar()
    {
        if (somClique != null && SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySFX(somClique);
        }
        ligado = !ligado;
        animator.SetBool("Ligado", ligado);
    }
}