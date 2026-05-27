using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource audioSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;

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
            sceneName == "GameOver"
        )
        {
            targetMusic = menuMusic;
        }

        // Cenas do jogo
        if (
            sceneName == "Jogo" ||
            sceneName == "JogoTutorial"
        )
        {
            targetMusic = gameMusic;
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
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;

        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);

        audioSource.mute = !musicOn;

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