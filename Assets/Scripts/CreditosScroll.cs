using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 50f;

    void Update()
    {
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }
}