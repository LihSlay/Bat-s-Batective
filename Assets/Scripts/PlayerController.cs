using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

   void Update()
{
    if (Input.GetKey(KeyCode.W))
    {
        Debug.Log("W carregado");
    }
}
}