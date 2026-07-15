using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static bool HasKey { get; private set; } = false;

    // Verdadeiro depois de o jogador ter apanhado a chave pelo menos uma vez.
    // Ao contrário de HasKey, NÃO volta a false ao pousar (F) — serve para a
    // Porta deixar de mostrar a pena/nota a partir daí.
    public static bool JaApanhada { get; private set; } = false;

    public Camera playerCamera;

    public Vector3 heldLocalPosition = new Vector3(0.25f, -0.15f, 0.4f);
    public Vector3 heldLocalRotation = new Vector3(0f, 0f, 0f);

    public AudioClip pickupSound;
    public GameObject pousarUI;

    private void Start()
    {
    }

    void Update()
    {
        if (HasKey && Input.GetKeyDown(KeyCode.F))
            Drop();
    }

    public void Pickup()
    {
        HasKey = true;
        JaApanhada = true;
        SFXManager.Instance.PlaySFX(pickupSound);
        if (pousarUI != null) pousarUI.SetActive(true);

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
        HasKey = false;
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
        HasKey = false;
        JaApanhada = false;
    }
}
