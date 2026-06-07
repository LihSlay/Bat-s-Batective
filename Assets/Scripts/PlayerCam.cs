using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Head Bob")]
    public float bobFrequency = 2f;
    public float bobAmplitude = 0.05f;
    public float bobSmoothing = 10f;

    float xRotation = 0f;
    float bobTimer = 0f;
    Vector3 defaultLocalPos;
    Rigidbody playerRb;

    // Flip
    bool upsideDown = false;
    float zRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        defaultLocalPos = transform.localPosition;
        playerRb = playerBody.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Corrige os controlos quando está invertido
        if (upsideDown)
        {
            mouseX *= -1f;
            mouseY *= -1f;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(
            xRotation,
            0f,
            zRotation
        );

        playerBody.Rotate(Vector3.up * mouseX);

        HandleBob();
    }

    public void SetUpsideDown(bool isUpsideDown)
    {
        upsideDown = isUpsideDown;
        zRotation = upsideDown ? 180f : 0f;
    }

    void HandleBob()
    {
        Vector3 flatVelocity = new Vector3(
            playerRb.linearVelocity.x,
            0f,
            playerRb.linearVelocity.z
        );

        bool isMoving = flatVelocity.magnitude > 0.1f;

        if (isMoving)
            bobTimer += Time.deltaTime * bobFrequency;
        else
            bobTimer = 0f;

        Vector3 targetPos = defaultLocalPos +
            new Vector3(
                0f,
                Mathf.Sin(bobTimer * Mathf.PI * 2f) * bobAmplitude,
                0f
            );

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * bobSmoothing
        );
    }
}