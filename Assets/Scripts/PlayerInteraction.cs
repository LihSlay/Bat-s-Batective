using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3f;

    private string lastObject = "";

    void Update()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            if (hit.collider.name != lastObject)
            {
                Debug.Log("Estou a olhar para: " + hit.collider.name);
                lastObject = hit.collider.name;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                NumberButton button =
                    hit.collider.GetComponent<NumberButton>();

                if (button != null)
                {
                    button.PressButton();
                }

                SafeHandle handle =
                    hit.collider.GetComponent<SafeHandle>();

                if (handle != null)
                {
                    handle.PullHandle();
                }
            }
        }
    }
}