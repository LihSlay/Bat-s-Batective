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

    [Header("Bloco de Notas")]
    [Tooltip("Entrada do bloco de notas que fica ativa depois de falar com o Rato com o armário/cadeado já aberto (ex.: PosDialogo).")]
    [SerializeField] private GameObject posDialogoEntry;

    private bool playerInside = false;
    private bool padlockSolved = false;
    private bool papersConfirmed = false;
    private bool startedByMe = false;

    [Header("Porta")]
    [SerializeField] private SlidingDoor porta;

    [Header("Som")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip somAcerto;

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
                    // Ainda faltam pap�is colocar
                    if (!paperPuzzleManager.AreAllPapersPlaced())
                    {
                        conversation = afterPadlockConversation;
                    }
                    // Todos colocados mas errados
                    else if (!paperPuzzleManager.IsPuzzleSolved())
                    {
                        papersConfirmed = false;
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
                startedByMe = true;
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

        // Só reage ao fim de um diálogo que este NPC (Rato) iniciou.
        if (startedByMe)
        {
            startedByMe = false;

            // Depois de falar com o Rato, e desde que o armário/cadeado já esteja
            // aberto, ativa a entrada PosDialogo no bloco de notas (só uma vez).
            if (padlockSolved &&
                posDialogoEntry != null &&
                BlocoNotasToggle.Instance != null)
            {
                BlocoNotasToggle.Instance.MostrarEntrada(posDialogoEntry);
            }
        }

        // Se o puzzle estiver resolvido, toca o som e abre a porta
        if (papersConfirmed && paperPuzzleManager.IsPuzzleSolved())
        {
            if (audioSource != null && somAcerto != null)
                audioSource.PlayOneShot(somAcerto);

            if (porta != null)
                porta.OpenDoor();
        }
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
