using UnityEngine;

public class DoorInteractionPuzzle : MonoBehaviour
{

    public PadlockPuzzle puzzle;

    public DoorAnimator leftDoor;
    public DoorAnimator rightDoor;

    private bool opened = false;

    public void Interact()
    {
        Debug.Log("Interagi com a porta!");
        if (opened)
            return;

        if (puzzle.IsSolved)
        {
            Debug.Log("Puzzle resolvido!");
            opened = true;

            leftDoor.OpenDoor();
            rightDoor.OpenDoor();
        }
        else
        {
            Debug.Log("As portas estão trancadas.");
        }
    }
}