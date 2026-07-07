using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource audioSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip creditsMusic;
    public AudioClip gameMusicLevel2;

    private bool musicOn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;

        audioSource.mute = !musicOn;
    }

    private void Start()
    {
        UpdateMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusic(scene.name);
    }

    void UpdateMusic(string sceneName)
    {
        AudioClip targetMusic = null;

        // Cenas do menu
        if (
            sceneName == "MenuInicial" ||
            sceneName == "SeleçãoNiveis" ||
            sceneName == "SeleçãoNiveis2" ||
            sceneName == "GameOver"
        )
        {
            targetMusic = menuMusic;
        }

        // Cenas do jogo
        if (
            sceneName == "Jogo"
        )
        {
            targetMusic = gameMusic;
        }

        if (sceneName == "Créditos")
        {
            targetMusic = creditsMusic;
        }
        if (sceneName == "Carruagem2")
        {
            targetMusic = gameMusicLevel2;
        }

        if (targetMusic != null && audioSource.clip != targetMusic)
        {
            audioSource.Stop();

            audioSource.clip = targetMusic;

            if (musicOn)
            {
                audioSource.Play();
            }
        }
        if (sceneName == "Cutscene")
        {
            audioSource.Stop();
            return;
        }


        if (targetMusic == null)
        {
            return;
        }
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;

        Debug.Log("MusicOn = " + musicOn);

        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);

        audioSource.mute = !musicOn;

        Debug.Log("Mute = " + audioSource.mute);

        if (musicOn && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public bool IsMusicOn()
    {
        return musicOn;
    }
}