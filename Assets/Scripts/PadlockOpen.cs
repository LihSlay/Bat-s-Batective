using UnityEngine;

public class PadlockOpen : MonoBehaviour
{
    public Transform shackle;

    public float targetY = 0.01f;
    public float speed = 2f;

    private bool opening = false;

    void Update()
    {
        if (opening)
        {
            Vector3 target = shackle.localPosition;
            target.y = targetY;

            shackle.localPosition = Vector3.MoveTowards(
                shackle.localPosition,
                target,
                speed * Time.deltaTime
            );
        }
    }

    public void Open()
    {
        opening = true;
    }
}