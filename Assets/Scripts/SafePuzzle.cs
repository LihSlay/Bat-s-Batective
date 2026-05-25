using UnityEngine;
using TMPro;

public class SafePuzzle : MonoBehaviour
{
    public Animator safeAnimator;
    public string correctCode = "1234";
    public TMP_Text codeDisplay;   // arrastar o CodeDisplay aqui no Inspector

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
}