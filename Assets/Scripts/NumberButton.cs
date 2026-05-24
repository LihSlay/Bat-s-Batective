using UnityEngine;

public class NumberButton : MonoBehaviour
{
    public string digit;

    public SafePuzzle safePuzzle;

    private void OnMouseDown()
    {
        safePuzzle.AddDigit(digit);
    }
}