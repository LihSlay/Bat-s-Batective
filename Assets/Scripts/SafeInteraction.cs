using UnityEngine;

public class SafeInteraction : MonoBehaviour
{
    [Header("Referências")]
    public Transform cameraZoomPoint;
    public Camera playerCamera;
    public MonoBehaviour playerController;
    public MonoBehaviour playerCam;
    public RectTransform crosshair;

    private Transform originalParent;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    private Vector3 lockedPosition;
    private Quaternion lockedRotation;

    private bool isZoomed = false;
    public bool IsZoomed => isZoomed;

    public void EnterZoom()
    {
        if (isZoomed) return;

        originalParent = playerCamera.transform.parent;
        originalLocalPosition = playerCamera.transform.localPosition;
        originalLocalRotation = playerCamera.transform.localRotation;

        if (playerController != null) playerController.enabled = false;
        if (playerCam != null) playerCam.enabled = false;

        lockedPosition = cameraZoomPoint.position;
        lockedRotation = cameraZoomPoint.rotation;

        playerCamera.transform.SetParent(null);
        playerCamera.transform.SetPositionAndRotation(lockedPosition, lockedRotation);

        // Rato livre mas invisível — o crosshair segue o cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        isZoomed = true;
    }

    public void ExitZoom()
    {
        if (!isZoomed) return;

        playerCamera.transform.SetParent(originalParent, false);
        playerCamera.transform.localPosition = originalLocalPosition;
        playerCamera.transform.localRotation = originalLocalRotation;

        if (playerController != null) playerController.enabled = true;
        if (playerCam != null) playerCam.enabled = true;

        // Crosshair volta ao centro
        if (crosshair != null)
            crosshair.anchoredPosition = Vector2.zero;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isZoomed = false;
    }

    void Update()
    {
        if (isZoomed && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitZoom();
        }
    }

    void LateUpdate()
    {
        if (!isZoomed) return;

        playerCamera.transform.SetPositionAndRotation(lockedPosition, lockedRotation);

        if (crosshair != null)
            crosshair.position = Input.mousePosition;
    }
}
