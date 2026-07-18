using UnityEngine;
using DialogueEditor;

public class ConversationStarterCar3 : MonoBehaviour
{
    [SerializeField] private NPCConversation beforePadlockConversation;
    [SerializeField] private NPCConversation afterPadlockConversation;
    [SerializeField] private NPCConversation ratoConversationRight;
    [SerializeField] private PaperPuzzleManager paperPuzzleManager;
    [SerializeField] private NPCConversation wrongOrderConversation;

    [SerializeField] private InteractionUI npcDescriptionText;
    [SerializeField] private MonoBehaviour playerController;
    [SerializeField] private MonoBehaviour playerCam;

    private bool playerInside = false;
    private bool padlockSolved = false;
    private bool papersConfirmed = false;

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
                Debug.Log("PadlockSolved: " + padlockSolved);
                Debug.Log("PuzzleSolved: " + paperPuzzleManager.IsPuzzleSolved());

                NPCConversation conversation;

                if (!padlockSolved)
                {
                    // Antes de abrir o cadeado
                    conversation = beforePadlockConversation;
                }
                else
                {
                    // Ainda faltam papéis colocar
                    if (!paperPuzzleManager.AreAllPapersPlaced())
                    {
                        conversation = afterPadlockConversation;
                    }
                    // Todos colocados mas errados
                    else if (!paperPuzzleManager.IsPuzzleSolved())
                    {
                        conversation = wrongOrderConversation;
                    }
                    // Todos colocados e certos
                    else
                    {
                        if (!papersConfirmed)
                        {
                            conversation = ratoConversationRight;
                            papersConfirmed = true;
                        }
                        else
                        {
                            conversation = afterPadlockConversation;
                        }
                    }
                }

                ConversationManager.Instance.StartConversation(conversation);
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

    public void SetPadlockSolved(bool solved)
    {
        padlockSolved = solved;
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
