using UnityEngine;

// Igual ao KeyPickup, mas com o seu próprio estado (HasBilhete) para NÃO
// interferir com a porta (que abre com KeyPickup.HasKey). Ao interagir, o
// bilhete fica no canto direito do ecrã e pode ser pousado com F.
public class BilhetePickup : MonoBehaviour
{
    public static bool HasBilhete { get; private set; } = false;

    // Verdadeiro depois de o jogador ter apanhado o bilhete pelo menos uma vez.
    // Ao contrário de HasBilhete, NÃO volta a false ao pousar (F) — serve para
    // deixar de mostrar a pena depois da primeira interação.
    public static bool JaInteragido { get; private set; } = false;

    public Camera playerCamera;

    public Vector3 heldLocalPosition = new Vector3(0.25f, -0.15f, 0.4f);
    public Vector3 heldLocalRotation = new Vector3(0f, 0f, 0f);

    public AudioClip pickupSound;
    public GameObject pousarUI;

    void Update()
    {
        if (HasBilhete && Input.GetKeyDown(KeyCode.F))
            Drop();
    }

    public void Pickup()
    {
        HasBilhete = true;
        JaInteragido = true;
        SFXManager.Instance.PlaySFX(pickupSound);
        if (pousarUI != null) pousarUI.SetActive(true);

        // Da primeira vez, ativa a entrada "Moveis" no bloco de notas.
        // (o próprio método ignora chamadas seguintes se já estiver ativa)
        if (BlocoNotasToggle.Instance != null)
            BlocoNotasToggle.Instance.MostrarMoveis();

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
        HasBilhete = false;
        if (pousarUI != null) pousarUI.SetActive(false);

        transform.SetParent(null);

        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 5f))
            transform.position = hit.point;
        else
            transform.position = cam.position + cam.forward * 1.5f;

        if (TryGetComponent<Collider>(out var col))
            col.enabled = true;

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = false;
    }

    private void OnDestroy()
    {
        HasBilhete = false;
        JaInteragido = false;
    }
}
