
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using System.Collections;

public class LanguageSelector : MonoBehaviour
{
    public void SetLanguage(string languageCode)
    {
        StartCoroutine(ChangeLanguage(languageCode));
    }

    private IEnumerator ChangeLanguage(string languageCode)
    {
        yield return LocalizationSettings.InitializationOperation;

        Locale locale = LocalizationSettings
            .AvailableLocales.GetLocale(languageCode);

        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;
        }
        else
        {
            Debug.LogWarning("Idioma não encontrado: " + languageCode);
        }
    }
}
