using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entrou algo no trigger: " + other.name);

        if (!other.CompareTag("Player"))
            return;

        Debug.Log("É o Player!");

        if (!KeyPickup.HasKey)
        {
            Debug.Log("Não tem a chave!");
            return;
        }

        Debug.Log("Tem a chave!");

        SceneManager.LoadScene("Créditos");
    }
}