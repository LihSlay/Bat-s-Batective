using UnityEngine;

public class SwitchButton : MonoBehaviour
{
    private Animator animator;

    public bool ligado;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("Ligado", ligado);
    }

    public void Alternar()
    {
        ligado = !ligado;
        animator.SetBool("Ligado", ligado);
    }
}