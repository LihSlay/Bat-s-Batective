using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    public Lever lever1;
    public Lever lever2;
    public Lever lever3;
    public Lever lever4;

    public SlidingDoor door;

    private bool solved = false;

    public void CheckPuzzle()
    {
        if (solved) return;

        bool correct =
            lever1.currentState == LeverState.Up &&
            lever2.currentState == LeverState.Middle &&
            lever3.currentState == LeverState.Up &&
            lever4.currentState == LeverState.Down;

        if (correct)
        {
            solved = true;
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        Debug.Log("Puzzle Resolvido!");

        if (door != null)
        {
            door.OpenDoor();
        }
    }
}