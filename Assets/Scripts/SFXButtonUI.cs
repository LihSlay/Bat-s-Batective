using UnityEngine;
using TMPro;

public class SFXButtonUI : MonoBehaviour
{
    public TMP_Text buttonText;

    void Start()
    {
        UpdateText();
    }

    public void ToggleSFX()
    {
        SFXManager.Instance.ToggleSFX();

        UpdateText();
    }

    void UpdateText()
    {
        buttonText.text =
            SFXManager.Instance.IsSFXOn()
            ? "ON"
            : "OFF";
    }
}