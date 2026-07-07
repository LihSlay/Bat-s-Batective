using UnityEngine;

public enum LeverState
{
    Up,
    Middle,
    Down
}

public class Lever : MonoBehaviour
{
    public LeverState currentState = LeverState.Middle;

    public Transform leverModel;
    public AudioClip leverSound;

    public float upAngle = -45f;
    public float middleAngle = 0f;
    public float downAngle = 45f;

    public void Interact()

    {
        if (SFXManager.Instance != null)
        {
            SFXManager.Instance.PlaySFX(leverSound);
        }
        switch (currentState)
        {
            case LeverState.Up:
                currentState = LeverState.Middle;
                break;

            case LeverState.Middle:
                currentState = LeverState.Down;
                break;

            case LeverState.Down:
                currentState = LeverState.Up;
                break;
        }

        UpdateVisual();

        LeverPuzzle puzzle = FindFirstObjectByType<LeverPuzzle>();

        if (puzzle != null)
        {
            puzzle.CheckPuzzle();
        }
    }

    void UpdateVisual()
    {
        float angle = 0;

        switch (currentState)
        {
            case LeverState.Up:
                angle = upAngle;
                break;

            case LeverState.Middle:
                angle = middleAngle;
                break;

            case LeverState.Down:
                angle = downAngle;
                break;
        }

        leverModel.localRotation = Quaternion.Euler(angle, 0, 0);
    }


}