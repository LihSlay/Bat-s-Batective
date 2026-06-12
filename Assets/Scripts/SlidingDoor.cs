using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3, 0);
    public float speed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private bool opening = false;

    void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    void Update()
    {
        if (opening)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                openPosition,
                speed * Time.deltaTime
            );
        }
    }

    public void OpenDoor()
    {
        opening = true;
    }
}