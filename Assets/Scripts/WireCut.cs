using UnityEngine;

public class WireCut : MonoBehaviour
{
    public GameObject fioCortado;

    private bool cortado = false;

    public void Cut()
    {
        if (cortado) return;

        cortado = true;

        gameObject.SetActive(false);
        fioCortado.SetActive(true);
    }
}