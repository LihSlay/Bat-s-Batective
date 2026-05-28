using UnityEngine;

public class TrainAmbient : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SFXManager.Instance.IsSFXOn())
        {
            if (audioSource.mute)
            {
                audioSource.mute = false;
            }
        }
        else
        {
            if (!audioSource.mute)
            {
                audioSource.mute = true;
            }
        }
    }
}