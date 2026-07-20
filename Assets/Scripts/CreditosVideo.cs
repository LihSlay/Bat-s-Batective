using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CreditosVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        // Esconde o cursor durante o vídeo dos créditos
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Creds.mp4");

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
        // Mostra o cursor ao voltar ao menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MenuInicial");
    }
}
