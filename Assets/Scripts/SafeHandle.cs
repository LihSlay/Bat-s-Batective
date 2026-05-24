using UnityEngine;

public class SafeHandle : MonoBehaviour
{
    public SafePuzzle safePuzzle;

    private void OnMouseDown()
    {
        safePuzzle.CheckCode();
    }
}