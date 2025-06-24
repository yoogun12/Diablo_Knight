using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;
    public AudioClip mainMenuBGM;
    public AudioClip gamePlayBGM;
    public AudioClip gameClearBGM;
    public AudioClip gameOverBGM;

    private string currentSceneName;

    void Awake()
    {
        // 싱글톤 처리
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        PlaySceneBGM(currentSceneName);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != currentSceneName)
        {
            currentSceneName = scene.name;
            PlaySceneBGM(scene.name);
        }
    }

    void PlaySceneBGM(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenu":
                PlayBGM(mainMenuBGM);
                break;
            case "GamePlay":
                PlayBGM(gamePlayBGM);
                break;
            case "GameClear":
                PlayBGM(gameClearBGM);
                break;
            case "GameOver":
                PlayBGM(gameOverBGM);
                break;
            default:
                audioSource.Stop();
                break;
        }
    }

    void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // 이미 재생 중이면 무시
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }

}