using UnityEngine;

public class WireCut : MonoBehaviour
{
    public GameObject fioCortado;

    private bool cortado = false;

    private void OnMouseDown()
    {
        // Só deixa cortar se o jogador tiver o alicate na mão
        if (!AlicatePickup.HasAlicate)
        {
            Debug.Log("É preciso ter o alicate na mão para cortar o fio.");
            return;
        }

        Cut();
    }

    public void Cut()
    {
        if (cortado) return;

        cortado = true;

        gameObject.SetActive(false);
        fioCortado.SetActive(true);

        Debug.Log("Fio cortado!");
    }
}