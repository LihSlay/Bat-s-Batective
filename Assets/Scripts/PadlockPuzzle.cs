using UnityEngine;

public class PadlockPuzzle : MonoBehaviour
{
    public PadlockWheel wheel1;
    public PadlockWheel wheel2;
    public PadlockWheel wheel3;
    public PadlockWheel wheel4;

    public int code1 = 3;
    public int code2 = 7;
    public int code3 = 2;
    public int code4 = 5;

    private bool opened = false;

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

            Debug.Log("CADEADO ABERTO!");
        }
    }
}