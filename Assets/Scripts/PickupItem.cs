using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public AudioClip pickupSound;

    public void Pickup()
    {
        // Toca som
        SFXManager.Instance.PlaySFX(pickupSound);

        // Desativa objeto
        gameObject.SetActive(false);
    }
}