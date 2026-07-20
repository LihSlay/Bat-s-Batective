using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerController playerController;
    public Camera playerCamera;
    public float interactDistance = 3f;
    public SafeInteraction safeInteraction;
    public InteractionUI safeHint;
    public InteractionUI safeHintInicial;
    public InteractionUI gradeHint;

    [Header("Cursor de nota (troca o \"+\" pela pena)")]
    public GameObject crosshairPadrao;   // o "+" no centro do ecrã
    public GameObject crosshairNota;     // a imagem penanotar

    private string lastObject = "";
    private bool lookingAtKey = false;
    private bool lookingAtBilhete = false;
    private bool lookingAtSafe = false;
    private bool lookingAtSafeInicial = false;
    private bool lookingAtGrade = false;
    private bool safeFirstInteraction = false;

    public static FlipZone CurrentLookedFlipZone;
    void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
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
        Bandejas bandejaOlhada = null;

        bool hittingSafe = false;
        bool hittingGrade = false;
        // Verdadeiro quando o jogador está a olhar para um objeto que mostra
        // nota (flipnumeros, numeros ou porta teste): troca o "+" pela pena.
        bool hoveringNota = false;

        // Bilhete marcado com "alcance ilimitado" (ex.: BilheteArmario) pode ser
        // lido a qualquer distância quando o jogador está ao contrário, ignorando
        // paredes e qualquer objeto pelo caminho (procura ao longo de todo o raio).
        if (PlayerController.IsUpsideDown)
        {
            BilheteNota bn = null;
            RaycastHit[] bilheteHits = Physics.RaycastAll(ray, Mathf.Infinity);
            foreach (RaycastHit bilheteHit in bilheteHits)
            {
                BilheteNota candidato = bilheteHit.collider.GetComponentInParent<BilheteNota>();
                if (candidato != null && candidato.alcanceIlimitado)
                {
                    bn = candidato;
                    break;
                }
            }

            if (bn != null && bn.PodeInteragir)
            {
                if (bn.NotaPorApontar) hoveringNota = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    bn.Interagir();
                    AtualizarCursorNota(hoveringNota, zoomed);
                    return;
                }
            }
        }

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Interação com fios: só se corta com o alicate na mão. Dentro do
            // zoom do painel a câmara está encostada ao painel, por isso procura
            // o fio ao longo de todo o raio (atravessa o collider do painel que
            // fica à frente); fora do zoom basta o primeiro collider atingido.
            WireCut wire = zoomed
                ? FindWireAlongRay(ray)
                : hit.collider.GetComponentInParent<WireCut>();

            if (wire != null && AlicatePickup.HasAlicate && Input.GetMouseButtonDown(0))
            {
                playerController?.PararPassos();
                wire.Cut();
                return;
            }
            //Porta Principal nível1
            DoorInteraction door = hit.collider.GetComponentInParent<DoorInteraction>();

            if (door != null && Input.GetKeyDown(KeyCode.E))
            {
                // Com chave: abre a porta e não mostra nota.
                // Sem chave: não sai daqui, deixa passar para o bloco da
                // PortaInteraction mais abaixo, que mostra a nota no bloco de notas.
                if (KeyPickup.HasKey)
                {
                    playerController?.PararPassos();
                    door.Interact();
                    return;
                }
            }

            {
                //Debug.Log("Estou a olhar para: " + hit.collider.name);
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

            // Pena só enquanto a nota (consoante a orientação) ainda não foi apontada.
            if (flipNumeros != null && BlocoNotasToggle.Instance != null &&
                !BlocoNotasToggle.Instance.NotaFlipNumerosApontada(PlayerController.IsUpsideDown))
                hoveringNota = true;

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

            // Só interativo com a visão noturna ligada, por isso a pena só
            // aparece nesse estado (sem visão noturna, premir E não faz nada).
            // E deixa de aparecer assim que a nota é apontada.
            if (numeros != null && NightVision.IsNightVisionOn && BlocoNotasToggle.Instance != null &&
                !BlocoNotasToggle.Instance.NotaNumerosApontada())
                hoveringNota = true;

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

            // Depois de apanhar a chave, a nota da porta deixa de fazer sentido
            // (mesmo que a chave volte a ser pousada com F).
            bool portaPorApontar = porta != null && !KeyPickup.JaApanhada;

            // Pena só enquanto a nota da porta ainda não foi apontada.
            if (portaPorApontar && BlocoNotasToggle.Instance != null &&
                !BlocoNotasToggle.Instance.NotaPortaApontada())
                hoveringNota = true;

            if (portaPorApontar && Input.GetKeyDown(KeyCode.E))
            {
                if (BlocoNotasToggle.Instance != null)
                    BlocoNotasToggle.Instance.MostrarPortaChave();
                return;
            }

            // Vitrine: ao premir E na primeira vez, mostra a entrada VitrineNota.
            VitrineInteraction vitrine = hit.collider.GetComponent<VitrineInteraction>();
            if (vitrine == null && hit.collider.transform.parent != null)
                vitrine = hit.collider.transform.parent.GetComponent<VitrineInteraction>();

            // Pena só enquanto a nota da vitrine ainda não foi apontada.
            if (vitrine != null && !vitrine.NotaApontada)
                hoveringNota = true;

            if (vitrine != null && Input.GetKeyDown(KeyCode.E))
            {
                vitrine.MostrarNota();
                return;
            }

            // Papel1..Papel9 (Carruagem 4): ao premir E, ativa a entrada
            // respetiva (pap1..pap9) no bloco de notas. A malha visível é filha
            // do papel, por isso procura o marcador ao longo da cadeia de pais.
            PapelNota papelNota = hit.collider.GetComponentInParent<PapelNota>();

            // Pena só enquanto a nota deste papel ainda não foi apontada.
            if (papelNota != null && papelNota.NotaPorApontar)
                hoveringNota = true;

            if (papelNota != null && Input.GetKeyDown(KeyCode.E))
            {
                papelNota.MostrarNota();
                // A nota ficou apontada: repõe já o "+".
                AtualizarCursorNota(false, zoomed);
                return;
            }

            //Cadeado da carrtuagem 3
            PadlockArrow arrow = hit.collider.GetComponent<PadlockArrow>();

            if (arrow == null)
            {
                arrow = hit.collider.GetComponentInParent<PadlockArrow>();
            }

            if (arrow != null)
            {
                // Debug.Log("Estou a clicar numa seta!");
                if (zoomed && Input.GetMouseButtonDown(0))
                {
                    playerController?.PararPassos();
                    arrow.Interact();
                    return;
                }
            }

            SwitchButton switchButton = hit.collider.GetComponent<SwitchButton>();

            if (switchButton == null)
            {
                switchButton = hit.collider.GetComponentInChildren<SwitchButton>();
            }

            if (switchButton == null)
            {
                switchButton = hit.collider.GetComponentInParent<SwitchButton>();
            }

            if (switchButton != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    playerController?.PararPassos();
                    switchButton.Alternar();
                    return;
                }
            }


            // Bilhetes-nota (BilheteMesa, BilheteLixo, BilheteBanco): por defeito
            // só com a visão noturna ligada; ao premir E ativa a entrada respetiva.
            BilheteNota bilheteNota = hit.collider.GetComponent<BilheteNota>();
            if (bilheteNota == null && hit.collider.transform.parent != null)
                bilheteNota = hit.collider.transform.parent.GetComponent<BilheteNota>();

            // Pena só enquanto for interagível e a nota ainda não tiver sido apontada.
            if (bilheteNota != null && bilheteNota.PodeInteragir && bilheteNota.NotaPorApontar)
                hoveringNota = true;

            if (bilheteNota != null && bilheteNota.PodeInteragir && Input.GetKeyDown(KeyCode.E))
            {
                bilheteNota.Interagir();
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

            // Bilhete: mesmo comportamento da chave (fica no canto do ecrã,
            // pousa-se com F), mas com estado próprio para não abrir a porta.
            BilhetePickup bilhete = hit.collider.GetComponent<BilhetePickup>();
            if (bilhete == null && hit.collider.transform.parent != null)
                bilhete = hit.collider.transform.parent.GetComponent<BilhetePickup>();

            if (bilhete != null && !BilhetePickup.HasBilhete)
            {
                // Mostra a pena só até à primeira interação. Depois de já ter sido
                // apanhado uma vez, mantém o cursor normal mesmo ao voltar a pegar.
                if (!BilhetePickup.JaInteragido)
                    hoveringNota = true;

                if (!lookingAtBilhete)
                {
                    Debug.Log("Estás a ver o Bilhete. Prime E para o apanhar.");
                    lookingAtBilhete = true;
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    bilhete.Pickup();
                    Debug.Log("Bilhete apanhado!");
                    lookingAtBilhete = false;
                    // Ao apanhar, o bilhete vai para a mão (collider desativado),
                    // por isso repõe já o cursor "+" normal em vez da pena.
                    AtualizarCursorNota(false, zoomed);
                    return;
                }
            }
            else
            {
                lookingAtBilhete = false;
            }

            // Martelo: apanha-se com E (fica na mão, pousa-se com F). Estado
            // próprio (HasMartelo) para não interferir com a chave/porta.
            MarteloPickup martelo = hit.collider.GetComponent<MarteloPickup>();
            if (martelo == null && hit.collider.transform.parent != null)
                martelo = hit.collider.transform.parent.GetComponent<MarteloPickup>();

            if (martelo != null && !MarteloPickup.HasMartelo)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    martelo.Pickup();
                    Debug.Log("Martelo apanhado!");
                    return;
                }
            }

            // Vidro: só quebra se o martelo estiver na mão. Ao premir E
            // quebra o vidro e toca o som. O collider do vidro é filho do objeto

            GlassBreak glass = hit.collider.GetComponent<GlassBreak>();

            if (glass == null)
            {
                glass = hit.collider.GetComponentInParent<GlassBreak>();
            }

            if (glass != null && Input.GetMouseButtonDown(0))
            {
                playerController?.PararPassos();
                glass.BreakGlass();
                return;
            }

            //
            //
            KeySlot slot = hit.collider.GetComponent<KeySlot>();

            if (slot == null)
            {
                slot = hit.collider.GetComponentInParent<KeySlot>();
            }

            if (slot != null && Input.GetMouseButtonDown(0))
            {
                // Se já existe uma chave na entrada, retira-a
                if (slot.Ocupado)
                {
                    playerController?.PararPassos();
                    slot.RetirarChave();
                }
                // Se a entrada está vazia e o jogador tem uma chave
                else if (ChavePickup.HasChave)
                {
                    playerController?.PararPassos();
                    slot.ColocarChave(ChavePickup.Segurada);
                }

                return;
            }

            //
            //
            ConfirmButton confirmButton = hit.collider.GetComponent<ConfirmButton>();

            if (confirmButton == null)
            {
                confirmButton = hit.collider.GetComponentInParent<ConfirmButton>();
            }

            // Só se pode carregar no botão de dentro do zoom do painel
            // (PainelZoomPoint). De fora, olhar para o BotaoDesativar e clicar
            // não faz nada — senão dava para acabar o nível à distância.
            if (confirmButton != null && zoomed)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    playerController?.PararPassos();
                    confirmButton.Confirmar();
                    return;
                }
            }

            // Alicate: apanha-se com E (fica na mão, pousa-se com F). A malha é
            // filha do objeto-raiz (Rigidbody), por isso procura o pickup ao
            // longo de toda a cadeia de pais. Estado próprio (HasAlicate).
            AlicatePickup alicate = hit.collider.GetComponentInParent<AlicatePickup>();

            if (alicate != null && !AlicatePickup.HasAlicate)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    alicate.Pickup();
                    Debug.Log("Alicate apanhado!");
                    return;
                }
            }

            // Chave1..3 (Carruagem 4): apanha-se com E e pousa-se com F. Só se
            // pode segurar uma de cada vez, por isso ignora enquanto já houver
            // outra chave na mão.
            ChavePickup chave = hit.collider.GetComponentInParent<ChavePickup>();

            if (chave != null && !ChavePickup.HasChave)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    chave.Pickup();
                    Debug.Log("Chave apanhada!");
                    return;
                }
            }

            // Papel1..4: apanha-se com E (fica na mão). Pousa-se com F no chão
            // ou com E numa bandeja. Só se pode ter um papel de cada vez, por
            // isso ignora enquanto já houver outro papel na mão (HasPapel).
            PapelPickup papel = hit.collider.GetComponent<PapelPickup>();
            if (papel == null && hit.collider.transform.parent != null)
                papel = hit.collider.transform.parent.GetComponent<PapelPickup>();

            if (papel != null && !PapelPickup.HasPapel)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    papel.Pickup();
                    Debug.Log("Papel apanhado!");
                    return;
                }
            }

            // Bandeja/Local com papel colocado: se a mão estiver vazia, ao premir
            // E volta a apanhar-se esse papel. (Colocar faz-se com F no PapelPickup.)
            PapelLocal papelLocal = hit.collider.GetComponentInParent<PapelLocal>();
            if (papelLocal != null && papelLocal.Ocupado && !PapelPickup.HasPapel)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    papelLocal.PapelColocado.Pickup();
                    Debug.Log("Papel reapanhado da bandeja!");
                    return;
                }
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
                    playerController?.PararPassos();
                    lever.Interact();
                    return;
                }
            }

            DoorInteractionPuzzle puzzleDoor = hit.collider.GetComponentInParent<DoorInteractionPuzzle>();

            if (puzzleDoor != null)
            {
                // Pena só depois do cadeado aberto e enquanto a nota do armário
                // ainda não tiver sido apontada.
                if (puzzleDoor.NotaPorApontar) hoveringNota = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    playerController?.PararPassos();
                    puzzleDoor.Interact();
                    // Se abriu, a nota ficou apontada: repõe já o "+".
                    AtualizarCursorNota(false, zoomed);
                    return;
                }
            }

            bandejaOlhada = hit.collider.GetComponent<Bandejas>();

            if (bandejaOlhada == null && hit.collider.transform.parent != null)
            {
                bandejaOlhada = hit.collider.transform.parent.GetComponent<Bandejas>();
            }



            hit.collider.transform.TryGetComponent(out SafeInteraction safeHit);
            if (safeHit == null && hit.collider.transform.parent != null)
                hit.collider.transform.parent.TryGetComponent(out safeHit);

            if (safeHit != null && !zoomed)
                hittingSafe = true;

            // Pena enquanto a nota do cofre/cadeado ainda não foi apontada: é a
            // primeira interação (o E que entra no zoom) que a aponta.
            if (hittingSafe && !safeFirstInteraction)
                hoveringNota = true;

            // Grade1: deteta o marcador GradeInteraction no collider ou no pai
            hit.collider.transform.TryGetComponent(out GradeInteraction gradeHit);
            if (gradeHit == null && hit.collider.transform.parent != null)
                hit.collider.transform.parent.TryGetComponent(out gradeHit);

            if (gradeHit != null && !zoomed)
                hittingGrade = true;

            if (Input.GetKeyDown(KeyCode.E) && safeHit != null && !safeHit.IsZoomed)
            {
                playerController?.PararPassos();
                safeHit.EnterZoom();

                if (!safeFirstInteraction)
                {
                    safeFirstInteraction = true;
                    if (BlocoNotasToggle.Instance != null)
                        BlocoNotasToggle.Instance.MostrarEntradaCofre();
                }

                // Acabámos de entrar no zoom: tira já a pena. O "+" fica por
                // conta do SafeInteraction, que o escondeu no EnterZoom.
                AtualizarCursorNota(false, true);
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
                lookingAtBilhete = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (ControladorOlhos.Instance.olhosFechados)
            {
                // Se já estão fechados, abre-os
                ControladorOlhos.Instance.AlternarOlhos();
            }
            else
            {
                // Se estiver a olhar para uma bandeja, usa o som dela
                if (bandejaOlhada != null)
                {
                    ControladorOlhos.Instance.AlternarOlhos(bandejaOlhada.fonteAudio);
                }
                else
                {
                    // Caso contrário, usa o som ambiente
                    ControladorOlhos.Instance.AlternarOlhos();
                }
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

        AtualizarCursorNota(hoveringNota, zoomed);
    }

    // Procura um fio (WireCut) ao longo de TODO o raio, do mais perto ao mais
    // longe, atravessando colliders que estejam à frente (ex.: o painel durante
    // o zoom). Devolve o primeiro fio encontrado ou null.
    private WireCut FindWireAlongRay(Ray ray)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, interactDistance);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (RaycastHit h in hits)
        {
            WireCut w = h.collider.GetComponentInParent<WireCut>();
            if (w != null) return w;
        }
        return null;
    }

    // Troca o cursor "+" pela imagem da pena quando o jogador olha para um
    // objeto interativo com nota. Durante o zoom do cofre não mexe no "+"
    // (é o SafeInteraction que o gere), apenas garante que a pena está escondida.
    private void AtualizarCursorNota(bool hoveringNota, bool zoomed)
    {
        if (zoomed)
        {
            if (crosshairNota != null) crosshairNota.SetActive(false);
            return;
        }

        if (crosshairNota != null) crosshairNota.SetActive(hoveringNota);
        if (crosshairPadrao != null) crosshairPadrao.SetActive(!hoveringNota);
    }
}