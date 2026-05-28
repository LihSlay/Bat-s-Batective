using UnityEngine;
using UnityEngine.UI;
using DialogueEditor;

public class CloseConversationButton : MonoBehaviour
{
    public void CloseConversation()
    {
        if (ConversationManager.Instance.IsConversationActive)
        {
            ConversationManager.Instance.EndConversation();
        }
    }
}