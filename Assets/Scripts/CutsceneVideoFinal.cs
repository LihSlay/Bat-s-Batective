using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.Localization.Settings;

public class CutsceneVideoFinal : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    [Header("Nomes dos ficheiros em StreamingAssets")]
    public string videoPT = "FINAL CUTSCENE GRV.mp4";
    public string videoEN = "finalcutscene.mp4";   // confirma se o ficheiro tem este nome

    [Header("Volume do vídeo em inglês (0 a 1)")]
    [Range(0f, 1f)] public float volumeEN = 0.4f;

    IEnumerator Start()
    {
        // Esconde o cursor durante a cutscene
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Espera que o sistema de localização esteja pronto
        yield return LocalizationSettings.InitializationOperation;

        string code = LocalizationSettings.SelectedLocale.Identifier.Code;
        bool isPT = code.StartsWith("pt");
        string fileName = isPT ? videoPT : videoEN;

        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, fileName);

        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 0);
        videoPlayer.targetTexture = rt;

        RawImage rawImage = gameObject.AddComponent<RawImage>();
        rawImage.texture = rt;

        videoPlayer.loopPointReached += OnVideoFinished;

        // Prepara o vídeo primeiro, para o volume ser aplicado antes de tocar
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
            yield return null;

        if (!isPT)
            AplicarVolume(volumeEN);

        videoPlayer.Play();
    }

    void AplicarVolume(float volume)
    {
        if (videoPlayer.audioOutputMode == VideoAudioOutputMode.AudioSource)
        {
            AudioSource src = videoPlayer.GetTargetAudioSource(0);
            if (src != null)
                src.volume = volume;
        }
        else if (videoPlayer.audioOutputMode == VideoAudioOutputMode.Direct)
        {
            videoPlayer.SetDirectAudioVolume(0, volume);
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        SceneManager.LoadScene("Creditos");
    }
}