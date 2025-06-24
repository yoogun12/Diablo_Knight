using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;

    // ���� BGM
    public AudioClip mainMenuBGM;
    public AudioClip gamePlayBGM;
    public AudioClip gameClearBGM;
    public AudioClip gameOverBGM;

    // ��Ȳ�� BGM
    public AudioClip warningBGM;
    public AudioClip bossBGM;
    public AudioClip shopBGM;

    private AudioClip previousBGM; // ���� ����Ǵ� ����� �����ϴ� ����
    private string currentSceneName;

    void Awake()
    {
        // �̱��� ó��
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
        if (audioSource.clip == clip) return; // �̹� ��� ���̸� ����
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    // ���� ��� ��������
    public float GetVolume()
    {
        return audioSource.volume;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    //  ���� ��� ���
    public void PlayShopBGM()
    {
        if (shopBGM == null) return;

        // ���� ��� ���� �� ��ü
        previousBGM = audioSource.clip;
        PlayBGM(shopBGM);
    }

    //  ���� ��� ����
    public void RestorePreviousBGM()
    {
        if (previousBGM != null)
        {
            PlayBGM(previousBGM);
        }
    }
}