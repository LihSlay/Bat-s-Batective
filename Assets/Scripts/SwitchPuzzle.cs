using UnityEngine;

public class SwitchPuzzle : MonoBehaviour
{
    public SwitchButton botao1;
    public SwitchButton botao2;
    public SwitchButton botao3;
    public SwitchButton botao4;

    // Combinação correta
    public bool corretoBotao1 = true;   // ON
    public bool corretoBotao2 = false;  // OFF
    public bool corretoBotao3 = false;  // OFF
    public bool corretoBotao4 = true;   // ON

    public bool EstaResolvido()
    {
        return
            botao1.ligado == corretoBotao1 &&
            botao2.ligado == corretoBotao2 &&
            botao3.ligado == corretoBotao3 &&
            botao4.ligado == corretoBotao4;
    }
}