using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;


    
    private Rigidbody rb;

    private bool upsideDown = false;
    private bool canFlip = false;
    private AudioSource footstepsAudio;


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
    IconActive.Instance?.SetFlipAtivo(upsideDown);

    Physics.gravity = upsideDown
        ? new Vector3(0, 9.81f, 0)
        : new Vector3(0, -9.81f, 0);

    // Limpa velocidades bugadas
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    // Guarda rotação horizontal atual
    float currentY = transform.eulerAngles.y;

    // Define rotação fixa correta
    if (upsideDown)
    {
        transform.rotation = Quaternion.Euler(0f, currentY, 180f);
    }
    else
    {
        transform.rotation = Quaternion.Euler(0f, currentY, 0f);
    }
}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ENTROU NO TRIGGER");

        if (other.CompareTag("Rail"))
        {
            Debug.Log("PODE VIRAR");

            canFlip = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("SAIU");

        if (other.CompareTag("Rail"))
        {
            canFlip = false;
        }
    }

    private void OnDisable() // Para garantir que o som dos passo pare se o player for desativado
    {
        if (footstepsAudio != null)
        {
            footstepsAudio.Stop();
        }
    }
}