using UnityEngine;
using UnityEngine.Localization.Settings;
using DialogueEditor;

[System.Serializable]
public class LocalizedConversation
{
    public NPCConversation pt;
    public NPCConversation en;

    public NPCConversation Get()
    {
        var locale = LocalizationSettings.SelectedLocale;
        string code = locale != null ? locale.Identifier.Code : "pt";

        if (code.StartsWith("en") && en != null)
            return en;

        return pt;
    }
}