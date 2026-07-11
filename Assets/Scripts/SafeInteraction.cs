using UnityEngine;
using UnityEngine.EventSystems;

public class SafeInteraction : MonoBehaviour
{
    [Header("Referências")]
    public Transform cameraZoomPoint;
    public Camera playerCamera;
    public MonoBehaviour playerController;
    public PlayerCam playerCam;
    public RectTransform crosshair;
    public GameObject exitZoomButton;

    [Header("Cursor")]
    public Texture2D cursorTexture;
    public Vector2 cursorHotspot = Vector2.zero;

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

        // Esconde o crosshair durante o zoom
        if (crosshair != null) crosshair.gameObject.SetActive(false);

        if (exitZoomButton != null) exitZoomButton.SetActive(true);

        isZoomed = true;

        // Rato livre e visível com o cursor personalizado dentro do zoom
        ApplyZoomCursor();
    }

    // Reaplica o estado do cursor do zoom (rato livre + textura personalizada).
    // Chamado também ao retomar do menu de pausa.
    public void ApplyZoomCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (cursorTexture != null)
            Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    public void ExitZoom()
    {
        if (!isZoomed) return;

        playerCamera.transform.SetParent(originalParent, false);
        playerCamera.transform.localPosition = originalLocalPosition;
        playerCamera.transform.localRotation = originalLocalRotation;

        if (playerController != null) playerController.enabled = true;
        if (playerCam != null) playerCam.enabled = true;

        // Crosshair volta ao centro e reaparece
        if (crosshair != null)
        {
            crosshair.anchoredPosition = Vector2.zero;
            crosshair.gameObject.SetActive(true);
        }

        // Repõe o cursor padrão do jogo (definido pelo CursorManager) e esconde-o
        if (CursorManager.Instance != null)
            CursorManager.Instance.ApplyDefaultCursor();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (exitZoomButton != null) exitZoomButton.SetActive(false);

        // Limpa a seleção para o botão "voltar atrás" não reaparecer preso no
        // estado realçado/pressionado da próxima vez que entramos no zoom.
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(null);

        isZoomed = false;
    }

    void LateUpdate()
    {
        if (!isZoomed) return;

        playerCamera.transform.SetPositionAndRotation(lockedPosition, lockedRotation);

        if (crosshair != null)
            crosshair.position = Input.mousePosition;
    }
}
