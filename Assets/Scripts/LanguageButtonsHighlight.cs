using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageButtonsHighlight : MonoBehaviour
{
    public Button ptButton;
    public Button enButton;

    private ColorBlock ptOriginal;
    private ColorBlock enOriginal;

    void Awake()
    {
        // Guarda as cores originais de cada botão
        ptOriginal = ptButton.colors;
        enOriginal = enButton.colors;
    }

    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += UpdateVisuals;

        if (LocalizationSettings.InitializationOperation.IsDone)
            UpdateVisuals(LocalizationSettings.SelectedLocale);
    }

    IEnumerator Start()
    {
        // Espera que o sistema de localização esteja pronto
        yield return LocalizationSettings.InitializationOperation;
        UpdateVisuals(LocalizationSettings.SelectedLocale);
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= UpdateVisuals;
    }

    void UpdateVisuals(Locale locale)
    {
        if (locale == null) return;

        bool isPT = locale.Identifier.Code.StartsWith("pt");

        Apply(ptButton, ptOriginal, isPT);
        Apply(enButton, enOriginal, !isPT);
    }

    void Apply(Button button, ColorBlock original, bool active)
    {
        ColorBlock c = original;

        if (active)
        {
            // O botão ativo fica sempre com a cor "pressed"
            c.normalColor = original.pressedColor;
            c.highlightedColor = original.pressedColor;
            c.selectedColor = original.pressedColor;
        }

        button.colors = c;
    }
}