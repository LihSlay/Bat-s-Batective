using UnityEngine;

public class BillboardText : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam == null)
            cam = Camera.main;

        transform.rotation = cam.transform.rotation;
    }
}