using UnityEngine;

public class NumberButton : MonoBehaviour
{
    public string digit;
    public SafePuzzle safePuzzle;

    public void PressButton()
    {
        safePuzzle.AddDigit(digit);
    }
}