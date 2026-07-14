using UnityEngine;

// Igual ao KeyPickup/BilhetePickup, mas partilhado por vários papéis (Papel1..4).
// O estado HasPapel é estático, por isso só se pode ter UM papel na mão de cada
// vez: enquanto houver um papel apanhado, não se pode apanhar outro. Ao pousar
// com F, liberta a mão e volta a permitir apanhar qualquer papel.
public class PapelPickup : MonoBehaviour
{
    // Verdadeiro enquanto qualquer papel estiver na mão do jogador.
    public static bool HasPapel => Held != null;

    // Referência ao papel atualmente na mão (null quando não há nenhum).
    public static PapelPickup Held { get; private set; } = null;

    public Camera playerCamera;

    public Vector3 heldLocalPosition = new Vector3(0.25f, -0.15f, 0.4f);
    public Vector3 heldLocalRotation = new Vector3(0f, 0f, 0f);

    // Distância máxima a que se consegue encaixar o papel num Local.
    public float snapDistance = 5f;

    // Quanto o papel é afastado da superfície ao pousar, para o colisor não
    // ficar cravado nela (evita o "salto" da física).
    public float dropSurfaceOffset = 0.05f;

    public AudioClip pickupSound;
    public AudioClip snapSound;
    public GameObject pousarUI;
    // UI "Atribuir" que aparece (tal como o pousarUI/"Sair") enquanto o papel
    // está na mão.
    public GameObject atribuirUI;

    // Local (slot da Mesa) onde este papel está encaixado, se algum.
    private PapelLocal currentLocal;

    // Frame em que o papel foi apanhado, para não processar a mesma tecla (E)
    // no próprio frame do pickup — senão o papel era logo re-encaixado.
    private int pickedFrame = -1;

    void Update()
    {
        // Só o papel que está na mão responde às teclas.
        if (Held != this) return;

        // Não processar teclas no frame em que foi apanhado: o E que o apanhou
        // (no PlayerInteraction) ainda está pressionado e voltaria a encaixá-lo.
        if (Time.frameCount == pickedFrame) return;

        // F pousa o papel normalmente (no chão / superfície onde se olha).
        if (Input.GetKeyDown(KeyCode.F))
            Drop();

        // E encaixa o papel numa bandeja (Local), se estiver a olhar para uma.
        // Se não estiver a olhar para nenhuma, não faz nada.
        if (Input.GetKeyDown(KeyCode.E))
            TrySnapToLocal();
    }

    public void Pickup()
    {
        // Já há um papel na mão: não deixa apanhar outro.
        if (Held != null) return;

        // Se estava encaixado num Local, liberta esse local ao voltar à mão.
        if (currentLocal != null)
        {
            currentLocal.Limpar();
            currentLocal = null;
        }

        Held = this;
        pickedFrame = Time.frameCount;

        if (pickupSound != null)
            SFXManager.Instance.PlaySFX(pickupSound);
        if (pousarUI != null) pousarUI.SetActive(true);
        if (atribuirUI != null) atribuirUI.SetActive(true);

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        if (TryGetComponent<Collider>(out var col))
            col.enabled = false;

        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;
        transform.SetParent(cam);
        transform.SetLocalPositionAndRotation(heldLocalPosition, Quaternion.Euler(heldLocalRotation));
    }

    public void Drop()
    {
        if (Held != this) return;

        Held = null;
        if (pousarUI != null) pousarUI.SetActive(false);
        if (atribuirUI != null) atribuirUI.SetActive(false);

        transform.SetParent(null);

        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;

        TryGetComponent<Collider>(out var col);

        if (TryLookHit(cam, 5f, out RaycastHit hit))
        {
            // Afasta o papel da superfície pela sua PRÓPRIA dimensão (não um
            // valor fixo): assim, mesmo inclinado / a olhar de lado, nenhuma
            // ponta do colisor fica cravada na superfície — é isso que causa
            // o impulso violento ("salto") da física.
            float clearance = dropSurfaceOffset;
            if (col != null) clearance += col.bounds.extents.magnitude;
            transform.position = hit.point + hit.normal * clearance;
        }
        else
        {
            transform.position = cam.position + cam.forward * 1.5f;
        }

        if (col != null) col.enabled = true;

        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            // Zera a velocidade acumulada enquanto esteve preso à câmara,
            // senão o papel é "cuspido" ao largar.
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    // Se o jogador estiver a olhar para um Local válido, encaixa lá o papel
    // (na posição/rotação exatas do Local) e devolve true. Caso contrário
    // devolve false, para que o F pouse o papel normalmente.
    private bool TrySnapToLocal()
    {
        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;

        if (!TryLookHit(cam, snapDistance, out RaycastHit hit))
            return false;

        PapelLocal local = hit.collider.GetComponentInParent<PapelLocal>();
        if (local == null || !local.Aceita(this))
            return false;

        Held = null;
        if (pousarUI != null) pousarUI.SetActive(false);
        if (atribuirUI != null) atribuirUI.SetActive(false);

        // Fica exatamente na posição/rotação do destino do Local e fixo (sem física).
        transform.SetParent(null);
        transform.SetPositionAndRotation(local.Destino.position, local.Destino.rotation);

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        // Colisor fica LIGADO para se poder voltar a apanhar o papel com E
        // (aponta-se ao próprio papel, que é o que se vê). Fica kinematic, por
        // isso não cai nem é empurrado.
        if (TryGetComponent<Collider>(out var col))
            col.enabled = true;

        currentLocal = local;
        local.Colocar(this);

        if (snapSound != null)
            SFXManager.Instance.PlaySFX(snapSound);

        return true;
    }

    // Lança um raio a partir da câmara e devolve o primeiro objeto atingido que
    // NÃO seja o próprio jogador (a câmara é filha dele) nem o próprio papel.
    // Evita que o papel seja pousado em cima do jogador quando se olha de lado
    // ou para baixo.
    private bool TryLookHit(Transform cam, float distance, out RaycastHit result)
    {
        result = default;

        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, distance, ~0, QueryTriggerInteraction.Ignore);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        Transform player = cam.root;
        foreach (RaycastHit h in hits)
        {
            // Ignora o jogador (mesma raiz da câmara) e o próprio papel.
            if (h.collider.transform.root == player) continue;
            if (h.collider.transform == transform || h.collider.transform.IsChildOf(transform)) continue;

            result = h;
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        if (Held == this)
            Held = null;
        if (currentLocal != null)
            currentLocal.Limpar();
    }
}
