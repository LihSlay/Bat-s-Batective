using UnityEngine;

public class DoorInteractionPuzzle : MonoBehaviour
{

    public PadlockPuzzle puzzle;

    public DoorAnimator leftDoor;
    public DoorAnimator rightDoor;

    [Tooltip("Objeto do bloco de notas que fica ativo ao abrir (ex.: PedidosArmario).")]
    public GameObject notaEntry;

    private bool opened = false;

    // Verdadeiro enquanto abrir o armário ainda der uma nota por apontar: só
    // conta depois do cadeado estar aberto, porque antes disso o E não faz nada.
    public bool NotaPorApontar =>
        !opened && puzzle != null && puzzle.IsSolved &&
        notaEntry != null && !notaEntry.activeSelf;

    public void Interact()
    {
        Debug.Log("Interagi com a porta!");
        if (opened)
            return;

        if (puzzle.IsSolved)
        {
            Debug.Log("Puzzle resolvido!");
            opened = true;

            leftDoor.OpenDoor();
            rightDoor.OpenDoor();

            if (BlocoNotasToggle.Instance != null)
                BlocoNotasToggle.Instance.MostrarEntrada(notaEntry);
        }
        else
        {
            Debug.Log("As portas est�o trancadas.");
        }
    }
}