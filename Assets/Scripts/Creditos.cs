using UnityEngine;
using TMPro;

public class CreditsFade : MonoBehaviour
{
    public float speed = 50f;
    public float fadeStartY = 300f;
    public float fadeSpeed = 0.2f;

    private TextMeshProUGUI textMesh;
    private Color color;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        color = textMesh.color;
    }

    void Update()
    {
        // Movimento para cima
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // Começa a desaparecer
        if (transform.position.y > fadeStartY)
        {
            color.a -= fadeSpeed * Time.deltaTime;

            // Impede alpha negativo
            color.a = Mathf.Clamp01(color.a);

            textMesh.color = color;
        }
    }
}