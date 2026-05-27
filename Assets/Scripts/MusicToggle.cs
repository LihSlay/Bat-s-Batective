using UnityEngine;
using TMPro;

public class MusicToggle : MonoBehaviour
{
    public TMP_Text buttonText;

    private AudioSource musicSource;

    private bool musicOn;

    void Start()
    {
        musicSource = FindFirstObjectByType<AudioSource>();

        // Ler estado guardado
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        ApplyMusicState();

        UpdateButtonText();
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;

        // Guardar estado
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);

        ApplyMusicState();

        UpdateButtonText();
    }

    void ApplyMusicState()
    {
        if (musicSource == null) return;

        musicSource.mute = !musicOn;

        if (musicOn && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    void UpdateButtonText()
    {
        buttonText.text = musicOn ? "ON" : "OFF";
    }
}