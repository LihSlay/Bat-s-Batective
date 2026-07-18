using UnityEngine;

// Pickup do Martelo, modelado no KeyPickup/BilhetePickup mas com estado próprio
// (HasMartelo) para NÃO interferir com a chave/porta. Apanha-se com E (fica
// preso à câmara, na mão) e pousa-se com F.
public class MarteloPickup : MonoBehaviour
{
    // Verdadeiro enquanto o martelo estiver na mão do jogador.
    public static bool HasMartelo { get; private set; } = false;

    public Camera playerCamera;

    public Vector3 heldLocalPosition = new Vector3(0.25f, -0.15f, 0.4f);
    public Vector3 heldLocalRotation = new Vector3(0f, 0f, 0f);

    public AudioClip pickupSound;
    public GameObject pousarUI;

    void Update()
    {
        if (!HasMartelo) return;

        // Dentro de um zoom (painel/cofre) não se pode pousar o objeto e o UI de
        // "pousar" fica escondido; ao sair do zoom volta a aparecer se ainda o
        // tivermos na mão.
        bool zoomed = SafeInteraction.AnyZoomed;
        if (pousarUI != null) pousarUI.SetActive(!zoomed);

        if (!zoomed && Input.GetKeyDown(KeyCode.F))
            Drop();
    }

    public void Pickup()
    {
        HasMartelo = true;

        if (pickupSound != null && SFXManager.Instance != null)
            SFXManager.Instance.PlaySFX(pickupSound);
        if (pousarUI != null) pousarUI.SetActive(true);

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        // O Martelo tem mais do que um colisor: desativa TODOS para não empurrar
        // o jogador nem voltar a ser apanhado enquanto está na mão.
        SetCollidersEnabled(false);

        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;
        transform.SetParent(cam);
        transform.SetLocalPositionAndRotation(heldLocalPosition, Quaternion.Euler(heldLocalRotation));
    }

    public void Drop()
    {
        HasMartelo = false;
        if (pousarUI != null) pousarUI.SetActive(false);

        transform.SetParent(null);

        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 5f))
            transform.position = hit.point;
        else
            transform.position = cam.position + cam.forward * 1.5f;

        SetCollidersEnabled(true);

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = false;
    }

    private void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider col in GetComponents<Collider>())
            col.enabled = enabled;
    }

    private void OnDestroy()
    {
        HasMartelo = false;
    }
}
