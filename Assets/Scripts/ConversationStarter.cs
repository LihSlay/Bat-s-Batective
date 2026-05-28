using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private InteractionUI npcDescriptionText;

    private bool playerInside = false;

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += ShowDescription;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= ShowDescription;
    }


    private void Update()
    {
        // Interação / avançar diálogo
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            // Se não estiver em conversa, inicia
            if (!ConversationManager.Instance.IsConversationActive)
            {
                ConversationManager.Instance.StartConversation(myConversation);

                Debug.Log("FadeOut chamado");

                npcDescriptionText.FadeOut();
            }
            else
            {
                // Avança o diálogo
                ConversationManager.Instance.SkipTyping();

                // Se não estava a escrever, avança normalmente
                ConversationManager.Instance.PressSelectedOption();
            }
        }

        // Fecha conversa com ESC
        if (ConversationManager.Instance.IsConversationActive &&
    Input.GetKeyDown(KeyCode.Q))
        {
            AudioSource audioSource =
                ConversationManager.Instance.GetComponent<AudioSource>();

            if (audioSource != null)
            {
                audioSource.Stop();
            }

            ConversationManager.Instance.EndConversation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private void ShowDescription()
    {
        if (playerInside)
        {
            npcDescriptionText.FadeIn();
        }
    }
}



