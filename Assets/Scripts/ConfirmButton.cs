using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    public PanelPuzzleManager panelPuzzleManager;

    public void Confirmar()
    {

        Debug.Log("Botão clicado!");
        panelPuzzleManager.ConfirmarPainel();
    }
}