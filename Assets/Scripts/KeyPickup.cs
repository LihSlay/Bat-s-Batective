using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static bool HasKey { get; private set; } = false;

    public Camera playerCamera;

    public Vector3 heldLocalPosition = new Vector3(0.25f, -0.15f, 0.4f);
    public Vector3 heldLocalRotation = new Vector3(0f, 0f, 0f);
    public float heldScale = 1f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (HasKey && Input.GetKeyDown(KeyCode.F))
            Drop();
    }

    public void Pickup()
    {
        HasKey = true;
        Debug.Log("Chave apanhada!");

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = true;

        if (TryGetComponent<Collider>(out var col))
            col.enabled = false;

        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;
        transform.SetParent(cam);
        transform.SetLocalPositionAndRotation(heldLocalPosition, Quaternion.Euler(heldLocalRotation));
        transform.localScale = Vector3.one * heldScale;
    }

    public void Drop()
    {
        HasKey = false;
        Debug.Log("Chave pousada!");

        transform.SetParent(null);

        Transform cam = playerCamera != null ? playerCamera.transform : Camera.main.transform;

        if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 5f))
            transform.position = hit.point;
        else
            transform.position = cam.position + cam.forward * 1.5f;

        transform.localScale = originalScale;

        if (TryGetComponent<Collider>(out var col))
            col.enabled = true;

        if (TryGetComponent<Rigidbody>(out var rb))
            rb.isKinematic = false;
    }

    private void OnDestroy()
    {
        HasKey = false;
    }
}
