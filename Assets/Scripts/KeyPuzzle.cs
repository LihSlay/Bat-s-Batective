using UnityEngine;

public class KeyPuzzle : MonoBehaviour
{
    public KeySlot fechadura1;
    public KeySlot fechadura2;
    public KeySlot fechadura3;

    public bool EstaResolvido()
    {
        return fechadura1.EstaCorreto() &&
               fechadura2.EstaCorreto() &&
               fechadura3.EstaCorreto();
    }
}