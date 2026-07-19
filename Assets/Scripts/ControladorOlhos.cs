using UnityEngine;
using System.Collections;

public class ControladorOlhos : MonoBehaviour
{
    public static ControladorOlhos Instance;

    [Header("Refer�ncias")]
    public PlayerController playerController;
    public PlayerCam playerCam;
    public AudioSource somAmbiente;

    private AudioSource somAtual;

    public bool olhosFechados = false;
    private bool emTransicao = false;

    void Awake()
    {
        Instance = this;
    }

    public void AlternarOlhos(AudioSource somEspecial = null)
    {
        if (emTransicao)
            return;

        StartCoroutine(AlternarOlhosCoroutine(somEspecial));
    }

    IEnumerator AlternarOlhosCoroutine(AudioSource somEspecial)
    {
        emTransicao = true;

        if (!olhosFechados)
        {
            // Fecha os olhos
            yield return StartCoroutine(ScreenFader.Instance.FadeOut(0.4f));

            olhosFechados = true;

            playerController.podeMover = false;
            playerCam.podeOlhar = false;

            if (somEspecial != null)
            {
                somAtual = somEspecial;
            }
            else
            {
                somAtual = somAmbiente;
            }

            if (somAtual != null && !somAtual.isPlaying)
            {
                somAtual.Play();
            }

            if (IconActive.Instance != null)
                IconActive.Instance.SetOuvirAtivo(true);
        }
        else
        {
            // Abre os olhos

            if (somAtual != null)
            {
                somAtual.Stop();
                somAtual = null;
            }

            playerController.podeMover = true;
            playerCam.podeOlhar = true;

            yield return StartCoroutine(ScreenFader.Instance.FadeIn(0.4f));

            olhosFechados = false;

            if (IconActive.Instance != null)
                IconActive.Instance.SetOuvirAtivo(false);
        }

        emTransicao = false;
    }
}