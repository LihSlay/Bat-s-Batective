using UnityEngine;

public class PadlockPuzzle : MonoBehaviour
{
    public PadlockWheel wheel1;
    public PadlockWheel wheel2;
    public PadlockWheel wheel3;
    public PadlockWheel wheel4;

    public PadlockOpen padlock;


    public PadlockAnimator padlockAnimator;

    public int code1 = 4;
    public int code2 = 8;
    public int code3 = 9;
    public int code4 = 3;

    private bool opened = false;
    public bool IsSolved => opened;
    public void CheckCode()
    {
        if (opened) return;

        bool correct =
            wheel1.currentNumber == code1 &&
            wheel2.currentNumber == code2 &&
            wheel3.currentNumber == code3 &&
            wheel4.currentNumber == code4;

        if (correct)
        {
            opened = true;

            // A argola sobe
            padlock.Open();

            // Meio segundo depois, o cadeado cai
            Invoke(nameof(DropPadlock), 0.5f);
        }
    }
    void DropPadlock()
    {
        if (padlockAnimator != null)
            padlockAnimator.Fall();
    }


}