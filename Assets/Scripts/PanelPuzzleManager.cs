using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PanelPuzzleManager : MonoBehaviour
{
    public SwitchPuzzle switchPuzzle;
    public WirePuzzle wirePuzzle;
    public KeyPuzzle keyPuzzle;
    public AudioSource audioSource;
    public AudioClip explosionSound;
    public Image fadePanel;

    public void ConfirmarPainel()
    {
        if (switchPuzzle.EstaResolvido() &&
            wirePuzzle.EstaResolvido() &&
            keyPuzzle.EstaResolvido())
        {
            SceneManager.LoadScene("Créditos");
        }
        else
        {
            // O som é opcional: se o AudioSource não estiver atribuído na cena,
            // o fim de jogo tem de acontecer na mesma.
            if (audioSource != null && explosionSound != null)
                audioSource.PlayOneShot(explosionSound);

            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
    {
        // O fade também é opcional: sem painel atribuído salta-se o escurecer,
        // mas nunca se salta a ida para o GameOver.
        if (fadePanel != null)
        {
            Color color = fadePanel.color;

            float duration = 3f;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;

                float alpha = timer / duration;

                fadePanel.color = new Color(
                    color.r,
                    color.g,
                    color.b,
                    alpha
                );

                yield return null;
            }
        }

        // Mesma razão do GameTimer: o cursor vem trancado e escondido do jogo,
        // e a cena GameOver não o solta sozinha.
        if (CursorManager.Instance != null)
            CursorManager.Instance.ApplyDefaultCursor();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("GameOver");
    }
}
