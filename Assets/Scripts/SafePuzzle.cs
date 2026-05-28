using UnityEngine;
using TMPro;

public class SafePuzzle : MonoBehaviour
{
    public Animator safeAnimator;
    public string correctCode = "1234";
    public TMP_Text codeDisplay;   
    public AudioClip safeOpenSound;
    public AudioClip correctCodeSound;

    private string currentCode = "";
    private bool opened = false;

    void Start()
    {
        UpdateDisplay();
    }

    public void AddDigit(string digit)
    {
        if (opened) return;

        currentCode += digit;
        if (currentCode.Length > 4)
        {
            currentCode = "";
        }

        Debug.Log(currentCode);
        UpdateDisplay();
    }

    public void CheckCode()
    {
        if (opened) return;

        if (currentCode == correctCode)
        {
            
            opened = true;
            SFXManager.Instance.PlaySFX(correctCodeSound);
            SFXManager.Instance.PlaySFX(safeOpenSound);
            safeAnimator.Play("SafeOpen");
            Debug.Log("Cofre aberto!");
        }
        else
        {
            Debug.Log("Código errado");
            currentCode = "";
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (codeDisplay == null) return;

        // Mostra os dígitos inseridos + traços para os que faltam
        // Ex: "12--" depois de inserir 1 e 2
        string display = currentCode;
        for (int i = currentCode.Length; i < correctCode.Length; i++)
        {
            display += "-";
        }
        codeDisplay.text = display;
    }

    public void ClearCode()
{
    if (currentCode.Length > 0)
    {
        currentCode =
            currentCode.Substring(0, currentCode.Length - 1);

        UpdateDisplay();
    }
}
}