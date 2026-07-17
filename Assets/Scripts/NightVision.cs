using UnityEngine;

public class NightVision : MonoBehaviour
{
    public GameObject volume;

    public GameObject[] hiddenObjects;

    private bool nightVisionOn = false;

    // Estado partilhado para outros scripts saberem se a visão noturna está ligada.
    public static bool IsNightVisionOn { get; private set; }

    void Update()
    {
        if (PauseMenu.IsGamePaused) return;

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            nightVisionOn = !nightVisionOn;
            IsNightVisionOn = nightVisionOn;

            volume.SetActive(nightVisionOn);

            foreach (GameObject obj in hiddenObjects)
            {
                obj.SetActive(nightVisionOn);
            }
        }
    }
}