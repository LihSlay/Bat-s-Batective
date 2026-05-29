using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Creditos");
        }
    }
}