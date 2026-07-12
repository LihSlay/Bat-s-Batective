using UnityEngine;

public class PadlockPuzzle : MonoBehaviour
{
    public PadlockWheel wheel1;
    public PadlockWheel wheel2;
    public PadlockWheel wheel3;
    public PadlockWheel wheel4;

    public PadlockOpen padlock;

    public DoorAnimator leftDoor;
    public DoorAnimator rightDoor;

    public int code1 = 4;
    public int code2 = 8;
    public int code3 = 9;
    public int code4 = 3;

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

            // Abre o cadeado
            padlock.Open();

            // Espera um pouco e abre as portas
            Invoke(nameof(OpenDoors), 0.6f);
        }
    }

    void OpenDoors()
    {
        if (leftDoor != null)
            leftDoor.OpenDoor();

        if (rightDoor != null)
            rightDoor.OpenDoor();
    }
}