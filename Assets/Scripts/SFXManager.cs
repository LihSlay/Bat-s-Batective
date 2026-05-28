using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioSource audioSource;

    private bool sfxOn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!sfxOn) return;

        audioSource.PlayOneShot(clip);
    }

    public void ToggleSFX()
    {
        sfxOn = !sfxOn;

        PlayerPrefs.SetInt("SFXOn", sfxOn ? 1 : 0);
    }

    public bool IsSFXOn()
    {
        return sfxOn;
    }
}