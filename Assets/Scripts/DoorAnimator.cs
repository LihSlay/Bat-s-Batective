using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        anim.SetTrigger("Open");
    }
}