using UnityEngine;

public class BlocoNotasToggle : MonoBehaviour
{
    public GameObject blocoNotasUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            bool aberto = !blocoNotasUI.activeSelf;
            blocoNotasUI.SetActive(aberto);

            Cursor.lockState = aberto ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = aberto;
        }
    }
}