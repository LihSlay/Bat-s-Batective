using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

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

        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
    }


    void Update()
    {
        if (!videoPlayer.isPlaying)
        {
            Debug.Log("Video parou!");
        }
    }



    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("MenuInicial");
    }
}
