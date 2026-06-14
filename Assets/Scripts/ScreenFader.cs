using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;

    public Image fadeImage;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator FadeOut(float duration)
    {
        float timer = 0;

        Color color = fadeImage.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(0f, 1f, timer / duration);
            fadeImage.color = color;

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }

    public IEnumerator FadeIn(float duration)
    {
        float timer = 0;

        Color color = fadeImage.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(1f, 0f, timer / duration);
            fadeImage.color = color;

            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
    }
}
