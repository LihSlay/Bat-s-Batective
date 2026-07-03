using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;
    public SafeInteraction safeInteraction;
    public InteractionUI safeHint;
    public InteractionUI safeHintInicial;
    public InteractionUI gradeHint;

    private string lastObject = "";
    private bool lookingAtKey = false;
    private bool lookingAtSafe = false;
    private bool lookingAtSafeInicial = false;
    private bool lookingAtGrade = false;
    private bool safeFirstInteraction = false;

    public static FlipZone CurrentLookedFlipZone;
    void Start()
    {
        if (safeHintInicial != null)
            safeHintInicial.FadeIn();
    }

    void Update()
    {
        bool zoomed = safeInteraction != null && safeInteraction.IsZoomed;
        Ray ray = zoomed
            ? playerCamera.ScreenPointToRay(Input.mousePosition)
            : playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        bool hittingSafe = false;
        bool hittingGrade = false;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {

            //Porta Principal nível1
            DoorInteraction door = hit.collider.GetComponent<DoorInteraction>();

            if (door == null && hit.collider.transform.parent != null)
            {
                door = hit.collider.transform.parent.GetComponent<DoorInteraction>();
            }

            if (door != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    door.Interact();
                    return;
                }
            }

            {
                Debug.Log("Estou a olhar para: " + hit.collider.name);
                lastObject = hit.collider.name;

                // Procura a FlipZone ao longo de TODO o raio (e nos pais do collider),
                // para que um objeto à frente (ex.: um item selecionável com outline)
                // não impeça de detetar a zona.
                CurrentLookedFlipZone = null;
                RaycastHit[] flipHits = Physics.RaycastAll(ray, interactDistance);
                foreach (RaycastHit flipHit in flipHits)
                {
                    FlipZone fz = flipHit.collider.GetComponentInParent<FlipZone>();
                    if (fz != null)
                    {
                        CurrentLookedFlipZone = fz;
                        break;
                    }
                }
            }

            // FlipNumeros: mostra no bloco de notas a entrada correta consoante
            // o jogador esteja na posição normal (chão) ou virado ao contrário.
            FlipNumerosInteraction flipNumeros = hit.collider.GetComponent<FlipNumerosInteraction>();
            if (flipNumeros == null && hit.collider.transform.parent != null)
                flipNumeros = hit.collider.transform.parent.GetComponent<FlipNumerosInteraction>();

            if (flipNumeros != null && Input.GetKeyDown(KeyCode.E))
            {
                if (BlocoNotasToggle.Instance != null)
                {
                    if (PlayerController.IsUpsideDown)
                        BlocoNotasToggle.Instance.MostrarNumContrarioCerto();
                    else
                        BlocoNotasToggle.Instance.MostrarNumContrarioErrado();
                }
                return;
            }

            // Numeros: só é possível interagir com a visão noturna ligada.
            // Ao premir E nesse estado, mostra a entrada NumVisaoCerto no bloco.
            NumerosInteraction numeros = hit.collider.GetComponent<NumerosInteraction>();
            if (numeros == null && hit.collider.transform.parent != null)
                numeros = hit.collider.transform.parent.GetComponent<NumerosInteraction>();

            if (numeros != null && NightVision.IsNightVisionOn && Input.GetKeyDown(KeyCode.E))
            {
                if (BlocoNotasToggle.Instance != null)
                    BlocoNotasToggle.Instance.MostrarNumVisaoCerto();
                return;
            }

            // Porta: ao premir E, mostra a entrada PortaChave no bloco de notas.
            PortaInteraction porta = hit.collider.GetComponent<PortaInteraction>();
            if (porta == null && hit.collider.transform.parent != null)
                porta = hit.collider.transform.parent.GetComponent<PortaInteraction>();

            if (porta != null && Input.GetKeyDown(KeyCode.E))
            {
                if (BlocoNotasToggle.Instance != null)
                    BlocoNotasToggle.Instance.MostrarPortaChave();
                return;
            }

            KeyPickup key = hit.collider.GetComponent<KeyPickup>();
            if (key != null && !KeyPickup.HasKey)
            {
                Debug.Log("Estou a olhar para: " + hit.collider.name);
                if (!lookingAtKey)
                {
                    Debug.Log("Estás a ver a Chave. Prime E para a apanhar.");
                    lookingAtKey = true;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    key.Pickup();
                    Debug.Log("Chave apanhada!");
                    lookingAtKey = false;
                    return;
                }
            }
            else
            {
                lookingAtKey = false;
            }

            Lever lever = hit.collider.GetComponent<Lever>();

            if (lever == null && hit.collider.transform.parent != null)
            {
                lever = hit.collider.transform.parent.GetComponent<Lever>();
            }

            if (lever != null)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    lever.Interact();
                    return;
                }
            }


            hit.collider.transform.TryGetComponent(out SafeInteraction safeHit);
            if (safeHit == null && hit.collider.transform.parent != null)
                hit.collider.transform.parent.TryGetComponent(out safeHit);

            if (safeHit != null && !zoomed)
                hittingSafe = true;

            // Grade1: deteta o marcador GradeInteraction no collider ou no pai
            hit.collider.transform.TryGetComponent(out GradeInteraction gradeHit);
            if (gradeHit == null && hit.collider.transform.parent != null)
                hit.collider.transform.parent.TryGetComponent(out gradeHit);

            if (gradeHit != null && !zoomed)
                hittingGrade = true;

            if (Input.GetKeyDown(KeyCode.E) && safeHit != null && !safeHit.IsZoomed)
            {
                safeHit.EnterZoom();

                if (!safeFirstInteraction)
                {
                    safeFirstInteraction = true;
                    if (BlocoNotasToggle.Instance != null)
                        BlocoNotasToggle.Instance.MostrarEntradaCofre();
                }

                return;
            }

            if (Input.GetMouseButtonDown(0) && safeInteraction != null && safeInteraction.IsZoomed)
            {
                if (hit.collider.TryGetComponent(out ClearButton clear)) clear.PressButton();
                if (hit.collider.TryGetComponent(out NumberButton button)) button.PressButton();
                if (hit.collider.TryGetComponent(out SafeHandle handle)) handle.PullHandle();
            }
        }
        else
        {
            CurrentLookedFlipZone = null;
            if (lastObject != "")
            {
                lastObject = "";
                lookingAtKey = false;
            }
        }

        if (safeHint != null)
        {
            if (hittingSafe && !lookingAtSafe)
            {
                safeHint.FadeIn();
                lookingAtSafe = true;
            }
            else if (!hittingSafe && lookingAtSafe)
            {
                safeHint.FadeOut();
                lookingAtSafe = false;
            }
        }

        if (safeHintInicial != null && safeFirstInteraction)
        {
            if (hittingSafe && !lookingAtSafeInicial)
            {
                safeHintInicial.FadeIn();
                lookingAtSafeInicial = true;
            }
            else if (!hittingSafe && lookingAtSafeInicial)
            {
                safeHintInicial.FadeOut();
                lookingAtSafeInicial = false;
            }
        }

        if (gradeHint != null)
        {
            if (hittingGrade && !lookingAtGrade)
            {
                gradeHint.FadeIn();
                lookingAtGrade = true;
            }
            else if (!hittingGrade && lookingAtGrade)
            {
                gradeHint.FadeOut();
                lookingAtGrade = false;
            }
        }

    }
}