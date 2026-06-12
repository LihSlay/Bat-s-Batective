using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    public Lever lever1;
    public Lever lever2;
    public Lever lever3;
    public Lever lever4;

    public GameObject door;

    private bool solved = false;

    public void CheckPuzzle()
    {
        if (solved) return;

        bool correct =
            lever1.currentState == LeverState.Up &&
            lever2.currentState == LeverState.Down &&
            lever3.currentState == LeverState.Down &&
            lever4.currentState == LeverState.Middle;

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
            door.SetActive(false);
        }
    }
}