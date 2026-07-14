using UnityEngine;

public class PadlockAnimator : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Fall()
    {
        anim.SetTrigger("Fall");
    }
}