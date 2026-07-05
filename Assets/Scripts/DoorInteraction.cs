using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public SlidingDoor slidingDoor;

    private bool opened = false;

    public void Interact()
    {
        if (opened)
            return;

        if (!KeyPickup.HasKey)
        {
            Debug.Log("Precisas da chave.");
            return;
        }

        opened = true;

        slidingDoor.OpenDoor();
    }
}