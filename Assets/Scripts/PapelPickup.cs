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

    public AudioClip pickupSound;
    public GameObject pousarUI;

    void Update()
    {
        // Só o papel que está na mão responde ao F para ser pousado.
        if (Held == this && Input.GetKeyDown(KeyCode.F))
            Drop();
    }

    public void Pickup()
    {
        // Já há um papel na mão: não deixa apanhar outro.
        if (Held != null) return;

        Held = this;

        if (pickupSound != null)
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
        if (Held != this) return;

        Held = null;
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
        if (Held == this)
            Held = null;
    }
}
