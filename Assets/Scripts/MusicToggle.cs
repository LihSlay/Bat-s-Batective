using UnityEngine;
using UnityEngine.Localization;
using TMPro;

public class MusicToggle : MonoBehaviour
{
    public TMP_Text buttonText;
    public LocalizedString onText;   // toggle_on
    public LocalizedString offText;  // toggle_off

    private LocalizedString current;

    void OnEnable()
    {
        UpdateButtonText();
    }

    void OnDisable()
    {
        if (current != null)
            current.StringChanged -= SetText;
        current = null;
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

        // Deixa de "ouvir" o texto anterior
        if (current != null)
            current.StringChanged -= SetText;

        // Escolhe o texto conforme o estado
        current = MusicManager.Instance.IsMusicOn() ? onText : offText;

        // Ao subscrever, o texto é atualizado logo e também
        // sempre que o jogador mudar de idioma
        current.StringChanged += SetText;
    }

    void SetText(string value)
    {
        buttonText.text = value;
    }
}