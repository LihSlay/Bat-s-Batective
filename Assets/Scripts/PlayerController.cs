using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;

    private bool upsideDown = false;
    private bool canFlip = false;
    private AudioSource footstepsAudio;
private Transform currentRail;
private Collider currentRailCollider;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        footstepsAudio = GetComponent<AudioSource>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Time.timeScale == 0)
        {
            if (footstepsAudio.isPlaying)
            {
                footstepsAudio.Stop();
            }
        }

        if (canFlip && Input.GetKeyDown(KeyCode.C))
        {
            FlipPlayer();
        }
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0)
        {
            if (footstepsAudio.isPlaying)
            {
                footstepsAudio.Stop();
            }

            return;
        }

        // Bloqueia movimento quando o player está upside down
        if (upsideDown)
        {
            // Mantém apenas a velocidade Y (gravidade) e zera movimento horizontal
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool playerMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        bool canPlayFootsteps =
            playerMoving &&
            SFXManager.Instance.IsSFXOn();

        if (canPlayFootsteps)
        {
            if (!footstepsAudio.isPlaying)
            {
                footstepsAudio.Play();
            }
        }
        else
        {
            if (footstepsAudio.isPlaying)
            {
                footstepsAudio.Stop();
            }
        }

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 targetVelocity = move * speed;

        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }

   void FlipPlayer()
{
    upsideDown = !upsideDown;

    // Diz à câmara para virar também
    PlayerCam cam = GetComponentInChildren<PlayerCam>();

    if (cam != null)
    {
        cam.SetUpsideDown(upsideDown);
    }

    Physics.gravity = upsideDown
        ? new Vector3(0, 9.81f, 0)
        : new Vector3(0, -9.81f, 0);

    // Limpa velocidades
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    // Guarda rotação horizontal atual
    float currentY = transform.eulerAngles.y;

    // Vira o player
    transform.rotation = Quaternion.Euler(
        0f,
        currentY,
        upsideDown ? 180f : 0f
    );

    // Cola o player à barra
    if (upsideDown && currentRailCollider != null)
    {
        Collider playerCollider = GetComponent<Collider>();

        Bounds railBounds = currentRailCollider.bounds;
        Bounds playerBounds = playerCollider.bounds;

        Vector3 pos = transform.position;

        float railBottom = railBounds.min.y;
        float playerHalfHeight = playerBounds.extents.y;

        pos.y = railBottom - playerHalfHeight;

        transform.position = pos;
    }

    Physics.SyncTransforms();
}
    private void OnTriggerEnter(Collider other)
{
    Debug.Log("ENTROU NO TRIGGER");

    if (other.CompareTag("Rail"))
    {
        Debug.Log("PODE VIRAR");

        canFlip = true;
        currentRail = other.transform;
        currentRailCollider = other;
    }
}

    private void OnTriggerExit(Collider other)
{
    Debug.Log("SAIU");

    if (other.CompareTag("Rail"))
    {
        canFlip = false;
        currentRail = null;
        currentRailCollider = null;
    }
}

    private void OnDisable()
    {
        if (footstepsAudio != null)
        {
            footstepsAudio.Stop();
        }
    }
}