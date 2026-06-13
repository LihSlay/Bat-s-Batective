using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float mouseSensitivity = 20f;
    public Transform playerBody;

    // ALTERAÇÃO: suaviza a rotação horizontal
    public float rotationSmoothSpeed = 12f;

    [Header("Head Bob")]
    public float bobFrequency = 2f;
    public float bobAmplitude = 0.05f;
    public float bobSmoothing = 10f;

    float xRotation = 0f;

    // ALTERAÇÃO: usado para virar a câmara quando o player fica upside down
    float zRotation = 0f;

    // ALTERAÇÃO: guarda estado upside down para corrigir controlos
    bool upsideDown = false;

    // ALTERAÇÃO: guarda rotação atual e alvo da rotação
    float yRotation = 0f;
    float targetYRotation = 0f;

    float bobTimer = 0f;
    Vector3 defaultLocalPos;
    Rigidbody playerRb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        defaultLocalPos = transform.localPosition;
        playerRb = playerBody.GetComponent<Rigidbody>();

        // ALTERAÇÃO: inicializa as rotações horizontais
        yRotation = playerBody.eulerAngles.y;
        targetYRotation = yRotation;
    }

    // ALTERAÇÃO: LateUpdate reduz jitter da câmara
    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // ALTERAÇÃO: corrige controlos quando está upside down
        // Esquerda continua esquerda, direita continua direita,
        // cima continua cima e baixo continua baixo.
        if (upsideDown)
        {
            mouseX *= -1f;
            mouseY *= -1f;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // ALTERAÇÃO: aplica também a rotação Z do flip
        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);

        // ALTERAÇÃO: rotação horizontal suavizada
        targetYRotation += mouseX;

        yRotation = Mathf.Lerp(
            yRotation,
            targetYRotation,
            rotationSmoothSpeed * Time.deltaTime
        );

        playerBody.rotation = Quaternion.Euler(0f, yRotation, 0f);

        HandleBob();
    }

    // ALTERAÇÃO: chamado pelo PlayerController quando faz flip
    public void SetUpsideDown(bool isUpsideDown)
    {
        upsideDown = isUpsideDown;
        zRotation = upsideDown ? 180f : 0f;
    }

    void HandleBob()
    {
        Vector3 flatVelocity = new(
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
