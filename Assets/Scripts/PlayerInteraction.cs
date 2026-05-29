using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;
    public SafeInteraction safeInteraction;
    public InteractionUI safeHint;

    private string lastObject = "";
    private bool lookingAtKey = false;
    private bool lookingAtSafe = false;
    private bool safeFirstInteraction = false;

    void Update()
    {
        bool zoomed = safeInteraction != null && safeInteraction.IsZoomed;
        Ray ray = zoomed
            ? playerCamera.ScreenPointToRay(Input.mousePosition)
            : playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        bool hittingSafe = false;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {

            {
                Debug.Log("Estou a olhar para: " + hit.collider.name);
                lastObject = hit.collider.name;
            }

            KeyPickup key = hit.collider.GetComponent<KeyPickup>();
            if (key != null && !KeyPickup.HasKey)
            {
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

            
            hit.collider.transform.TryGetComponent(out SafeInteraction safeHit);
            if (safeHit == null && hit.collider.transform.parent != null)
                hit.collider.transform.parent.TryGetComponent(out safeHit);

            if (safeHit != null && !zoomed)
                hittingSafe = true;

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
    }
}
