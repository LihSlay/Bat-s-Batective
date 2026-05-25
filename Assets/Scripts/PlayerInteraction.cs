using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;
    public SafeInteraction safeInteraction; // arrastar o Safe no Inspector

    private string lastObject = "";

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

            // E no cofre → entrar no modo zoom (só funciona quando ainda não está em zoom)
            if (Input.GetKeyDown(KeyCode.E))
            {
                SafeInteraction safe = hit.collider.GetComponentInParent<SafeInteraction>();
                if (safe != null && !safe.IsZoomed)
                {
                    safe.EnterZoom();
                    return;
                }
            }

            // Clique esquerdo nos botões / manípulo — só durante o zoom
            if (Input.GetMouseButtonDown(0) && safeInteraction != null && safeInteraction.IsZoomed)
            {
                NumberButton button = hit.collider.GetComponent<NumberButton>();
                if (button != null)
                {
                    button.PressButton();
                }

                SafeHandle handle = hit.collider.GetComponent<SafeHandle>();
                if (handle != null)
                {
                    handle.PullHandle();
                }
            }
        }
    }
}