using UnityEngine;

public class PaperPuzzleManager : MonoBehaviour
{
    [SerializeField] private PapelLocal[] paperSlots;

    public bool IsPuzzleSolved()
    {
        foreach (PapelLocal slot in paperSlots)
        {
            Debug.Log(
                $"{slot.name} | " +
                $"Ocupado: {slot.Ocupado} | " +
                $"Correto: {slot.EstaCorreto} | " +
                $"Colocado: {(slot.PapelColocado != null ? slot.PapelColocado.name : "Nenhum")} | " +
                $"Esperado: {(slot.papelCorreto != null ? slot.papelCorreto.name : "Nenhum")}"
            );

            if (!slot.Ocupado)
                return false;

            if (!slot.EstaCorreto)
                return false;
        }

        return true;
    }

    public bool AreAllPapersPlaced()
    {
        foreach (PapelLocal slot in paperSlots)
        {
            if (!slot.Ocupado)
                return false;
        }

        return true;
    }
}