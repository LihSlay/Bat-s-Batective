using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    public void Interact()
    {
        if (!KeyPickup.HasKey)
        {
            Debug.Log("Não tem a chave!");
            return;
        }

        SceneManager.LoadScene("Créditos");
    }
}