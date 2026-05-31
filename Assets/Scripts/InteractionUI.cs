using System.Collections;
using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    public float fadeSpeed = 3f;
    public bool startVisible = false;

    private Camera mainCam;
    private TextMeshPro textMesh;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        mainCam = Camera.main;
        textMesh = GetComponent<TextMeshPro>();
        Color c = textMesh.color;
        c.a = startVisible ? 1f : 0f;
        textMesh.color = c;
    }

    private void Update()
    {
        // Billboard effect
        transform.forward = mainCam.transform.forward;
    }

    public void FadeIn()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeText(1));
    }

    public void FadeOut()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeText(0));
    }

    IEnumerator FadeText(float targetAlpha)
    {
        Color currentColor = textMesh.color;

        while (!Mathf.Approximately(currentColor.a, targetAlpha))
        {
            currentColor.a = Mathf.MoveTowards(
                currentColor.a,
                targetAlpha,
                fadeSpeed * Time.deltaTime
            );

            textMesh.color = currentColor;

            yield return null;
        }
    }
}