using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 targetVelocity = move * speed;
        // preserva a velocidade vertical (gravidade)
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;
    }
}
