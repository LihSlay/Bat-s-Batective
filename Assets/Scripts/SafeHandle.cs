using UnityEngine;

public class SafeHandle : MonoBehaviour
{
    public SafePuzzle safePuzzle;

    public void PullHandle()
    {
        safePuzzle.CheckCode();
    }
}