using UnityEngine;

public class ClearButton : MonoBehaviour
{
    public SafePuzzle safePuzzle;

    public void PressButton()
    {
        safePuzzle.ClearCode();
    }
}