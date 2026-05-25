using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

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

        transform.Rotate(0, 0, 180);
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
        if (other.CompareTag("Rail"))
        {
            Debug.Log("SAIU");

            canFlip = false;
        }
    }
}
