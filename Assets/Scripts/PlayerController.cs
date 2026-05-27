using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    // Objetos dos puzzles
    public GameObject normalNumbers;
    public GameObject flipNumbers;

    private Rigidbody rb;

    private bool upsideDown = false;
    private bool canFlip = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (canFlip && Input.GetKeyDown(KeyCode.C))
        {
            FlipPlayer();
        }
    }

    void FixedUpdate()
    {
        // Bloqueia movimento quando o player está upside down
        if (upsideDown)
        {
            // Mantém apenas a velocidade Y (gravidade) e zera movimento horizontal
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 targetVelocity = move * speed;

        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }

    void FlipPlayer()
    {
        upsideDown = !upsideDown;

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

            // Ativa números upside down
            normalNumbers.SetActive(false);
            flipNumbers.SetActive(true);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, currentY, 0f);

            // Volta aos números normais
            normalNumbers.SetActive(true);
            flipNumbers.SetActive(false);
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
}