using UnityEngine;

// Pickup do Alicate, modelado no MarteloPickup mas com estado próprio
// (HasAlicate). O Alicate é composto por um objeto-raiz (Rigidbody + colisor) e
// uma malha filha com o seu próprio colisor, por isso desativa os colisores de
// TODA a hierarquia enquanto está na mão. Apanha-se com E (fica preso à câmara)
// e pousa-se com F.
public class AlicatePickup : MonoBehaviour
{
    // Verdadeiro enquanto o alicate estiver na mão do jogador.
    public static bool HasAlicate { get; private set; } = false;

    public Camera playerCamera;

    public Vector3 heldLocalPosition = new Vector3(0.25f, -0.15f, 0.4f);
    public Vector3 heldLocalRotation = new Vector3(0f, 0f, 0f);

    public AudioClip pickupSound;
    public GameObject pousarUI;

    void Update()
    {
        if (!HasAlicate) return;

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
        HasAlicate = true;

        if (pickupSound != null && SFXManager.Instance != null)
            SFXManager.Instance.PlaySFX(pickupSound);
        if (pousarUI != null) pousarUI.SetActive(true);

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        SetCollidersEnabled(false);

        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;
        transform.SetParent(cam);
        transform.SetLocalPositionAndRotation(heldLocalPosition, Quaternion.Euler(heldLocalRotation));
    }

    public void Drop()
    {
        HasAlicate = false;
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

    // Ativa/desativa TODOS os colisores da hierarquia (raiz + malha filha), para
    // o alicate não empurrar o jogador nem ser reapanhado enquanto está na mão.
    private void SetCollidersEnabled(bool enabled)
    {
        foreach (Collider col in GetComponentsInChildren<Collider>(true))
            col.enabled = enabled;
    }

    private void OnDestroy()
    {
        HasAlicate = false;
    }
}
