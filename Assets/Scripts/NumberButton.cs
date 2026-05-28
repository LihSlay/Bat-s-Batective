using UnityEngine;

public class NumberButton : MonoBehaviour
{
    public string digit;
    public AudioClip buttonSound;
    public SafePuzzle safePuzzle;

    public void PressButton()
    {
        SFXManager.Instance.PlaySFX(buttonSound);
        safePuzzle.AddDigit(digit);
    }
}