using UnityEngine;
using TMPro;

public class MusicToggle : MonoBehaviour
{
    public TMP_Text buttonText;

    void Start()
    {
        UpdateButtonText();
    }

    public void ToggleMusic()
    {
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.ToggleMusic();
        }

        UpdateButtonText();
    }

    void UpdateButtonText()
    {
        if (MusicManager.Instance == null)
            return;

        buttonText.text =
            MusicManager.Instance.IsMusicOn()
            ? "ON"
            : "OFF";
    }
}