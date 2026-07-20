using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    public Lever lever1;
    public Lever lever2;
    public Lever lever3;
    public Lever lever4;

    [Header("Objetos do BlocoNotas que têm de estar ativos")]
    // Canvas -> FundoBlocoNotasUI -> BlocoNotas -> ...
    public GameObject armarioFlip;
    public GameObject armarioNormal;
    public GameObject mesa;
    public GameObject banco;
    public GameObject lixo;

    public SlidingDoor door;
    public AudioClip puzzleSolvedSound;

    [Tooltip("Trigger de passagem para a carruagem seguinte (NextLevel). Começa " +
             "inativo na cena e só liga quando a porta abre, para não se poder " +
             "atravessar antes de resolver o puzzle.")]
    public GameObject nextLevel;

    private bool solved = false;

    public void CheckPuzzle()
    {
        if (solved) return;

        // A porta só abre se todas as pistas do BlocoNotas já estiverem ativas.
        if (!AllNotesActive()) return;

        bool correct =
            lever1.currentState == LeverState.Up &&
            lever2.currentState == LeverState.Middle &&
            lever3.currentState == LeverState.Up &&
            lever4.currentState == LeverState.Down;

        if (correct)
        {
            solved = true;
            if (SFXManager.Instance != null)
            {
                SFXManager.Instance.PlaySFX(puzzleSolvedSound);
            }
            OpenDoor();
        }
    }

    // Verdadeiro só quando os 5 objetos do BlocoNotas estão ativos.
    bool AllNotesActive()
    {
        return armarioFlip != null && armarioFlip.activeSelf &&
               armarioNormal != null && armarioNormal.activeSelf &&
               mesa != null && mesa.activeSelf &&
               banco != null && banco.activeSelf &&
               lixo != null && lixo.activeSelf;
    }

    void OpenDoor()
    {
        Debug.Log("Puzzle Resolvido!");

        if (door != null)
        {
            door.OpenDoor();
        }

        if (nextLevel != null)
        {
            nextLevel.SetActive(true);
        }
    }
}