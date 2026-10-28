using UnityEngine;
using UnityEngine.Localization;
using TMPro;

public class SFXButtonUI : MonoBehaviour
{
    public TMP_Text buttonText;
    public LocalizedString onText;   // toggle_on
    public LocalizedString offText;  // toggle_off

    private LocalizedString current;

    void OnEnable()
    {
        UpdateText();
    }

    void OnDisable()
    {
        if (current != null)
            current.StringChanged -= SetText;
        current = null;
    }

    public void ToggleSFX()
    {
        SFXManager.Instance.ToggleSFX();

        UpdateText();
    }

    void UpdateText()
    {
        if (SFXManager.Instance == null)
            return;

        if (current != null)
            current.StringChanged -= SetText;

        current = SFXManager.Instance.IsSFXOn() ? onText : offText;

        current.StringChanged += SetText;
    }

    void SetText(string value)
    {
        buttonText.text = value;
    }
}