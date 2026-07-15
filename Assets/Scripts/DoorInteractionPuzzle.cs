using UnityEngine;

public class DoorInteractionPuzzle : MonoBehaviour
{
    public PadlockPuzzle puzzle;

    public DoorAnimator leftDoor;
    public DoorAnimator rightDoor;

    [Header("Sons")]
    public AudioClip lockedDoorSound;

    [Header("Bloco de Notas")]
    [Tooltip("Objeto do bloco de notas que fica ativo ao abrir (ex.: PedidosArmario).")]
    public GameObject notaEntry;

    private bool opened = false;

    // Verdadeiro enquanto abrir o armário ainda der uma nota por apontar.
    public bool NotaPorApontar =>
        !opened &&
        puzzle != null &&
        puzzle.IsSolved &&
        notaEntry != null &&
        !notaEntry.activeSelf;

    public void Interact()
    {
        if (opened)
            return;

        if (puzzle != null && puzzle.IsSolved)
        {
            opened = true;

            leftDoor.OpenDoor();
            rightDoor.OpenDoor();

            if (BlocoNotasToggle.Instance != null)
                BlocoNotasToggle.Instance.MostrarEntrada(notaEntry);
        }
        else
        {
            if (lockedDoorSound != null)
            {
                SFXManager.Instance.PlaySFX(lockedDoorSound);
            }
        }
    }
}