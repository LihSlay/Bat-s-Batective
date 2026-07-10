using UnityEngine;
using TMPro;

public class PadlockWheel : MonoBehaviour
{
    public int currentNumber = 0;
    public TMP_Text numberText;

    void Start()
    {
        UpdateNumber();
    }

    public void Interact()
    {
        currentNumber++;

        if (currentNumber > 9)
            currentNumber = 0;

        UpdateNumber();

        PadlockPuzzle puzzle = FindFirstObjectByType<PadlockPuzzle>();

        if (puzzle != null)
        {
            puzzle.CheckCode();
        }
    }

    void UpdateNumber()
    {
        if (numberText != null)
            numberText.text = currentNumber.ToString();
    }
}