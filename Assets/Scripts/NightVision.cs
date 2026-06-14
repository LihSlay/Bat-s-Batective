using UnityEngine;

public class NightVision : MonoBehaviour
{
    public GameObject volume;

    public GameObject[] hiddenObjects;

    private bool nightVisionOn = false;

    void Update()
    {
        if (PauseMenu.IsGamePaused) return;

        if (Input.GetKeyDown(KeyCode.V))
        {
            nightVisionOn = !nightVisionOn;

            volume.SetActive(nightVisionOn);

            foreach (GameObject obj in hiddenObjects)
            {
                obj.SetActive(nightVisionOn);
            }
        }
    }
}