using UnityEngine;

public class PaperPuzzleManager : MonoBehaviour
{
    [SerializeField] private PapelLocal[] paperSlots;

    public bool IsPuzzleSolved()
    {
        foreach (PapelLocal slot in paperSlots)
        {
            if (!slot.Ocupado)
                return false;

            if (!slot.EstaCorreto)
                return false;
        }

        return true;
    }
}