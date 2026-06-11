using UnityEngine;
using DialogueEditor;

public class ConversationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private InteractionUI npcDescriptionText;
    [SerializeField] private MonoBehaviour playerController;
    [SerializeField] private MonoBehaviour playerCam;

    private bool playerInside = false;

    private void OnEnable()
    {
        ConversationManager.OnConversationEnded += OnConversationEnded;
    }

    private void OnDisable()
    {
        ConversationManager.OnConversationEnded -= OnConversationEnded;
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (!ConversationManager.Instance.IsConversationActive)
            {
                ConversationManager.Instance.StartConversation(myConversation);
                npcDescriptionText.FadeOut();
                SetPlayerLocked(true);
            }
            else
            {
                ConversationManager.Instance.SkipTyping();
                ConversationManager.Instance.PressSelectedOption();
            }
        }

        if (ConversationManager.Instance.IsConversationActive && Input.GetKeyDown(KeyCode.F))
        {
            AudioSource audioSource = ConversationManager.Instance.GetComponent<AudioSource>();
            if (audioSource != null) audioSource.Stop();
            ConversationManager.Instance.EndConversation();
        }
    }

    private void OnConversationEnded()
    {
        SetPlayerLocked(false);

        if (playerInside)
            npcDescriptionText.FadeIn();
    }

    private void SetPlayerLocked(bool locked)
    {
        if (playerController != null) playerController.enabled = !locked;
        if (playerCam != null) playerCam.enabled = !locked;

        if (locked && playerController != null)
        {
            Rigidbody rb = playerController.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}
