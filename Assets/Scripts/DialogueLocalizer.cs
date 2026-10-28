using UnityEngine.Localization.Settings;

public static class DialogueLocalizer
{
    public const string TableName = "Dialogos";

    public static string Translate(string text)
    {
        if (string.IsNullOrEmpty(text) || !text.StartsWith("@"))
            return text; // texto normal, não mexe

        string key = text.Substring(1).Trim();
        string result = LocalizationSettings.StringDatabase
            .GetLocalizedString(TableName, key);

        return string.IsNullOrEmpty(result) ? text : result;
    }
}