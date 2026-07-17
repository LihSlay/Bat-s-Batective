using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HoldToExitCutsceneRaposa : MonoBehaviour
{
    public float holdDuration = 2f;
    public Graphic sairText;
    public float baseAlpha = 0.3f;

    private float holdTimer = 0f;

    void Start()
    {
        SetAlpha(baseAlpha);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.deltaTime;
            SetAlpha(Mathf.Lerp(baseAlpha, 1f, holdTimer / holdDuration));

            if (holdTimer >= holdDuration)
            {
                // Mantém o cursor escondido ao saltar a cutscene
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SceneManager.LoadScene("Carruagem2");
            }
        }
        else
        {
            holdTimer = 0f;
            SetAlpha(baseAlpha);
        }
    }

    void SetAlpha(float alpha)
    {
        if (sairText == null) return;
        Color c = sairText.color;
        c.a = alpha;
        sairText.color = c;
    }
}
