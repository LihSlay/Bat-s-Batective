using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody rb;
    private bool upsideDown = false;
    private bool isFlipping = false;

    private FlipZone currentFlipZone;

    private Vector3 savedPosition;
    private Quaternion savedRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isFlipping)
        {
            StartCoroutine(FlipRoutine());
        }
    }

    void FixedUpdate()
    {
        if (upsideDown || isFlipping)
            return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Vector3 targetVelocity = move * speed;

        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = targetVelocity;
    }

    IEnumerator FlipRoutine()
    {
        isFlipping = true;

        yield return StartCoroutine(
            ScreenFader.Instance.FadeOut(0.2f)
        );

        FlipPlayer();

        yield return StartCoroutine(
            ScreenFader.Instance.FadeIn(0.2f)
        );

        isFlipping = false;
    }

    void FlipPlayer()
    {
        currentFlipZone = PlayerInteraction.CurrentLookedFlipZone;

        if (currentFlipZone == null)
        {
            Debug.Log("Não estás a olhar para nenhuma FlipZone.");
            return;
        }

        if (currentFlipZone.hangPoint == null)
        {
            Debug.LogError("O HangPoint não está atribuído em " + currentFlipZone.name);
            return;
        }

        upsideDown = !upsideDown;

        PlayerCam cam = Camera.main.GetComponent<PlayerCam>();

        if (upsideDown)
        {
            savedPosition = transform.position;
            savedRotation = transform.rotation;

            transform.position =
                currentFlipZone.hangPoint.position + Vector3.down * 0.80f;

            if (cam != null)
            {
                cam.SetUpsideDown(true);
            }

            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        else
        {
            transform.position = savedPosition;
            transform.rotation = savedRotation;

            if (cam != null)
            {
                cam.SetUpsideDown(false);
            }

            rb.isKinematic = false;
        }
    }
}