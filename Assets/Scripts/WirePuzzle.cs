using UnityEngine;

public class WirePuzzle : MonoBehaviour
{
    public WireCut fio1;
    public WireCut fio2;
    public WireCut fio3;
    public WireCut fio4;
    public WireCut fio5;
    public WireCut fio6;

    public bool EstaResolvido()
    {
        return
            fio1.Cortado &&
            !fio2.Cortado &&
            fio3.Cortado &&
            fio4.Cortado &&
            !fio5.Cortado &&
            fio6.Cortado;
    }
}