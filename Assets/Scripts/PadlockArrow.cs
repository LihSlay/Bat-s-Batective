using UnityEngine;

public class PadlockArrow : MonoBehaviour
{
    public PadlockWheel wheel;
    public bool increase;

    public void Interact()
    {
        if (increase)
            wheel.NextNumber();
        else
            wheel.PreviousNumber();
    }
}