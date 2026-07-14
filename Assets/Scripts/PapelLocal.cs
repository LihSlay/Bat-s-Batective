using UnityEngine;

// Marca a zona clicável (ex.: a Bandeja) onde um papel pode ser encaixado.
// Quando o jogador tem um papel na mão e prime F a olhar para este objeto, o
// papel fica exatamente na posição/rotação do "destino" (ex.: Local1).
// NOTA: o objeto precisa de um Collider para o raio da câmara o conseguir detetar.
public class PapelLocal : MonoBehaviour
{
    [Tooltip("Ponto exato onde o papel fica (ex.: Local1). Se vazio, usa este próprio objeto.")]
    public Transform destino;

    // Transform onde o papel deve encaixar (o destino, ou este objeto se não houver).
    public Transform Destino => destino != null ? destino : transform;

    // Papel atualmente encaixado neste local (null se estiver vazio).
    public PapelPickup PapelColocado { get; private set; }

    public bool Ocupado => PapelColocado != null;

    [Tooltip("Papel correto para esta bandeja (ex.: Papel1 para o Local1). Aceita qualquer papel, mas só este conta como correto.")]
    public PapelPickup papelCorreto;

    // Verdadeiro quando o papel colocado é o correto para esta bandeja.
    public bool EstaCorreto => PapelColocado != null && PapelColocado == papelCorreto;

    // Aceita qualquer papel, desde que a bandeja ainda esteja vazia.
    public bool Aceita(PapelPickup papel)
    {
        return !Ocupado;
    }

    public void Colocar(PapelPickup papel)
    {
        PapelColocado = papel;
    }

    public void Limpar()
    {
        PapelColocado = null;
    }
}
