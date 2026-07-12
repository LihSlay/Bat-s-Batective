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

    public void NextNumber()
    {
        currentNumber++;

        if (currentNumber > 9)
            currentNumber = 0;

        UpdateNumber();
    }

    public void PreviousNumber()
    {
        currentNumber--;

        if (currentNumber < 0)
            currentNumber = 9;

        UpdateNumber();
    }

    void UpdateNumber()
    {
        if (numberText != null)
            numberText.text = currentNumber.ToString();

        PadlockPuzzle puzzle = FindFirstObjectByType<PadlockPuzzle>();

        if (puzzle != null)
            puzzle.CheckCode();
    }
}