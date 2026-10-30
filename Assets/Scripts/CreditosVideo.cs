using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class CreditosVideo : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    [Header("Nomes dos ficheiros em StreamingAssets")]
    public string videoPT = "Creds_pt.mp4";
    public string videoEN = "Creds_en.mp4";

    IEnumerator Start()
    {
        // Esconde o cursor durante o vídeo dos créditos
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Espera que o sistema de localização esteja pronto
        yield return LocalizationSettings.InitializationOperation;

        string code = LocalizationSettings.SelectedLocale.Identifier.Code;
        string fileName = code.StartsWith("pt") ? videoPT : videoEN;

        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);
        videoPlayer.targetTexture = rt;

        RawImage rawImage = gameObject.AddComponent<RawImage>();
        rawImage.texture = rt;

        videoPlayer.loopPointReached += OnVideoFinished;

        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Mostra o cursor ao voltar ao menu
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MenuInicial");
    }
}