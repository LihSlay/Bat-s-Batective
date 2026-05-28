using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;

    public void PlayClick()
    {
        SFXManager.Instance.PlaySFX(clickSound);
    }
}