using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SceneManagement;
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
            audioSource.PlayOneShot(explosionSound);

            StartCoroutine(FadeAndLoad());
        }
    }

    IEnumerator FadeAndLoad()
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

        SceneManager.LoadScene("GameOver");
    }
}