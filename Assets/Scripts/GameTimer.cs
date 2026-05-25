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