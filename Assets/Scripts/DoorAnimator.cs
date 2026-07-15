using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    private Animator anim;

    public AudioClip openDoorSound;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        anim.SetTrigger("Open");

        if (openDoorSound != null)
        {
            SFXManager.Instance.PlaySFX(openDoorSound, 0.10f);
        }
    }
}