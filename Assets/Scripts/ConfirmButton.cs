using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    public PanelPuzzleManager panelPuzzleManager;

    public void Confirmar()
    {
        panelPuzzleManager.ConfirmarPainel();
    }
}