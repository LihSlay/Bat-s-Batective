using UnityEngine;

public class SlidingDoubleDoor : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    public Transform leftTarget;
    public Transform rightTarget;

    public float speed = 2f;

    private bool opening = false;

    void Update()
    {
        if (!opening) return;

        leftDoor.position = Vector3.MoveTowards(
            leftDoor.position,
            leftTarget.position,
            speed * Time.deltaTime);

        rightDoor.position = Vector3.MoveTowards(
            rightDoor.position,
            rightTarget.position,
            speed * Time.deltaTime);
    }

    public void OpenDoors()
    {
        opening = true;
    }
}