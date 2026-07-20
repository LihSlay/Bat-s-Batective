using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public bool podeMover = true;

    private Rigidbody rb;
    private bool upsideDown = false;
    private bool isFlipping = false;
    private AudioSource footstepsAudio;

    // Estado partilhado para outros scripts saberem se o jogador está virado
    // ao contrário (pendurado numa FlipZone) ou na posição normal (chão).
    public static bool IsUpsideDown { get; private set; }

    private FlipZone currentFlipZone;

    private Vector3 savedPosition;
    private Quaternion savedRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        footstepsAudio = GetComponent<AudioSource>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (PauseMenu.IsGamePaused) return;

        if (Input.GetKeyDown(KeyCode.Alpha2) && !isFlipping)
        {
            StartCoroutine(FlipRoutine());
        }
    }

    void FixedUpdate()
    {
        if (upsideDown || isFlipping || !podeMover)
        {
            rb.linearVelocity = Vector3.zero;

            if (footstepsAudio.isPlaying)
                footstepsAudio.Stop();

            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        bool playerMoving = Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f;

        bool canPlayFootsteps =
            playerMoving &&
            !PauseMenu.IsGamePaused &&
            SFXManager.Instance != null &&
            SFXManager.Instance.IsSFXOn();

        if (canPlayFootsteps)
        {
            if (!footstepsAudio.isPlaying)
                footstepsAudio.Play();
        }
        else
        {
            if (footstepsAudio.isPlaying)
                footstepsAudio.Stop();
        }

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
    PlayerCam cam = Camera.main.GetComponent<PlayerCam>();

    // Se já está pendurado, volta ao normal
    if (upsideDown)
    {
        upsideDown = false;
        IsUpsideDown = false;

        transform.position = savedPosition;
        transform.rotation = savedRotation;

        if (cam != null)
        {
            cam.SetUpsideDown(false);
        }

        rb.isKinematic = false;

        // Volta ao normal — ícone de flip desativado
        IconActive.Instance?.SetFlipAtivo(false);

        return;
    }

    // Se está normal, precisa de estar a olhar para uma FlipZone
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

    upsideDown = true;
    IsUpsideDown = true;

    savedPosition = transform.position;
    savedRotation = transform.rotation;

    transform.position =
        currentFlipZone.hangPoint.position + Vector3.down * 0.80f;

    if (cam != null)
    {
        cam.SetUpsideDown(true);
    }

    // Está virado numa FlipZone — ícone de flip ativo
    IconActive.Instance?.SetFlipAtivo(true);

    rb.linearVelocity = Vector3.zero;
    rb.isKinematic = true;
}
    public void PararPassos()
    {
        if (footstepsAudio != null && footstepsAudio.isPlaying)
        {
            footstepsAudio.Stop();
        }
    }

}

