using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;
    public SafeInteraction safeInteraction;

    private string lastObject = "";
    private bool lookingAtKey = false;

    void Update()
    {
        bool zoomed = safeInteraction != null && safeInteraction.IsZoomed;
        Ray ray = zoomed
            ? playerCamera.ScreenPointToRay(Input.mousePosition)
            : playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.name != lastObject)
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                SafeInteraction safe = hit.collider.GetComponentInParent<SafeInteraction>();
                if (safe != null && !safe.IsZoomed)
                {
                    safe.EnterZoom();
                    return;
                }
            }

            if (Input.GetMouseButtonDown(0) && safeInteraction != null && safeInteraction.IsZoomed)
            {
                ClearButton clear =
                 hit.collider.GetComponent<ClearButton>();

                if (clear != null)
                {
                    clear.PressButton();
                }
                NumberButton button = hit.collider.GetComponent<NumberButton>();
                if (button != null)
                    button.PressButton();

                SafeHandle handle = hit.collider.GetComponent<SafeHandle>();
                if (handle != null)
                    handle.PullHandle();
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
    }
}
