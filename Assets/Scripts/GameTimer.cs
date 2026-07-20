using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 30f;

    public TMP_Text timerText;

    public AudioSource audioSource;

    public AudioClip explosionSound;

    public Image fadePanel;

    private bool finished = false;

    void Update()
    {
        if (finished) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining < 0)
            timeRemaining = 0;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeRemaining <= 0)
        {
            finished = true;

            // O som é opcional: se o AudioSource não estiver atribuído na cena,
            // o fim de jogo tem de acontecer na mesma. Sem esta verificação, a
            // exceção rebentava antes do StartCoroutine e, como o finished já
            // estava a true, o nível continuava a correr para sempre.
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

        LiberarCursorParaMenu();

        SceneManager.LoadScene("GameOver");
    }

    // O estado do cursor persiste entre cenas: durante o jogo fica trancado e
    // escondido (PlayerCam), e na cena GameOver não há nada que o volte a
    // soltar. Sem isto, os botões do GameOver ficavam inclicáveis.
    private void LiberarCursorParaMenu()
    {
        // Repõe a textura padrão, caso se tenha perdido dentro de um zoom
        // (o SafeInteraction troca o cursor enquanto o zoom está ativo).
        if (CursorManager.Instance != null)
            CursorManager.Instance.ApplyDefaultCursor();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}