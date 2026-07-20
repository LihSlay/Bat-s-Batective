using UnityEngine;

// Pickup das chaves da Carruagem 4 (Chave1, Chave2, Chave3). Modelado no
// MarteloPickup, mas como há várias chaves iguais o estado não é um simples
// bool: guarda-se qual a chave que está na mão (Segurada), para só se poder
// segurar uma de cada vez. Apanha-se com E e pousa-se com F.
public class ChavePickup : MonoBehaviour
{
    // A chave que está neste momento na mão do jogador (null se nenhuma).
    public static ChavePickup Segurada { get; private set; } = null;

    // Verdadeiro enquanto o jogador tiver alguma chave na mão.
    public static bool HasChave => Segurada != null;

    public Camera playerCamera;

    public Vector3 heldLocalPosition = new Vector3(0.25f, -0.15f, 0.4f);
    public Vector3 heldLocalRotation = new Vector3(0f, 0f, 0f);

    public AudioClip pickupSound;
    public GameObject pousarUI;

    public KeyType tipoChave;

    void Update()
    {
        if (Segurada != this) return;

        // Dentro de um zoom (painel/cofre) não se pode pousar a chave e o UI de
        // "pousar" fica escondido; ao sair do zoom volta a aparecer.
        bool zoomed = SafeInteraction.AnyZoomed;
        if (pousarUI != null) pousarUI.SetActive(!zoomed);

        if (!zoomed && Input.GetKeyDown(KeyCode.F))
            Drop();
    }

    public void Pickup()
    {
        // Só uma chave de cada vez: se já houver outra na mão, ignora.
        if (Segurada != null) return;

        Segurada = this;

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
        if (Segurada == this) Segurada = null;
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
        if (Segurada == this) Segurada = null;
    }
}
