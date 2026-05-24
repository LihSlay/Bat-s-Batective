using UnityEngine;

public class SafePuzzle : MonoBehaviour
{
    public Animator safeAnimator;

    public string correctCode = "1234";

    private string currentCode = "";

    private bool opened = false;

    public void AddDigit(string digit)
    {
        if (opened) return;

        currentCode += digit;

        if (currentCode.Length > 4)
        {
            currentCode = "";
        }

        Debug.Log(currentCode);
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
        }
    }
}