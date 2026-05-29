using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CreditosVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);
        videoPlayer.targetTexture = rt;

        RawImage rawImage = gameObject.AddComponent<RawImage>();
        rawImage.texture = rt;

        videoPlayer.Play();
    }
}
