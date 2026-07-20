using UnityEngine;

public class KeySlot : MonoBehaviour
{
    public Transform pontoColocacao;

    private ChavePickup chaveColocada;

    public KeyType chaveCorreta;
    public bool Ocupado => chaveColocada != null;

    public void ColocarChave(ChavePickup chave)
    {
        if (Ocupado)
            return;

        chave.Drop();

        chave.transform.position = pontoColocacao.position;
        chave.transform.rotation = pontoColocacao.rotation;

        chave.transform.SetParent(pontoColocacao);

        Rigidbody rb = chave.GetComponent<Rigidbody>();

        if (rb != null)
            rb.isKinematic = true;

        chaveColocada = chave;
    }

    public void RetirarChave()
    {
        Debug.Log("RetirarChave chamado");
        if (!Ocupado)
            return;

        chaveColocada.transform.SetParent(null);

        Rigidbody rb = chaveColocada.GetComponent<Rigidbody>();

        if (rb != null)
            rb.isKinematic = false;

        chaveColocada.Pickup();

        chaveColocada = null;
    }

    public bool EstaCorreto()
    {
        if (chaveColocada == null)
            return false;

        return chaveColocada.tipoChave == chaveCorreta;
    }
}