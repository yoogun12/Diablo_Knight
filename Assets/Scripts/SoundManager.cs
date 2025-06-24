using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;

    // 씬별 BGM
    public AudioClip mainMenuBGM;
    public AudioClip gamePlayBGM;
    public AudioClip gameClearBGM;
    public AudioClip gameOverBGM;

    // 상황별 BGM
    public AudioClip warningBGM;
    public AudioClip bossBGM;
    public AudioClip shopBGM;

    private AudioClip previousBGM; // 원래 재생되던 브금을 저장하는 변수
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

    public void PlayBGM(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // 이미 재생 중이면 무시
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    // 현재 브금 가져오기
    public float GetVolume()
    {
        return audioSource.volume;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    //  상점 브금 재생
    public void PlayShopBGM()
    {
        if (shopBGM == null) return;

        // 현재 브금 저장 후 교체
        previousBGM = audioSource.clip;
        PlayBGM(shopBGM);
    }

    //  이전 브금 복원
    public void RestorePreviousBGM()
    {
        if (previousBGM != null)
        {
            PlayBGM(previousBGM);
        }
    }
}