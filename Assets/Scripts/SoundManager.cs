using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmAudioSource;    // ������ǿ� AudioSource
    public AudioSource sfxAudioSource;    // ȿ������ AudioSource

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
                bgmAudioSource.Stop();
                break;
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmAudioSource.clip == clip) return; // �̹� ��� ���̸� ����
        bgmAudioSource.clip = clip;
        bgmAudioSource.loop = true;
        bgmAudioSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxAudioSource.PlayOneShot(clip, sfxAudioSource.volume);
    }

    // BGM ���� ��������
    public float GetVolume()
    {
        return bgmAudioSource.volume;
    }

    // BGM ���� �����ϱ�
    public void SetVolume(float value)
    {
        bgmAudioSource.volume = value;
        sfxAudioSource.volume = value; // ȿ���� ������ ���� ����
    }

    // ���� ��� ���
    public void PlayShopBGM()
    {
        if (shopBGM == null) return;

        previousBGM = bgmAudioSource.clip;
        PlayBGM(shopBGM);
    }

    // ���� ��� ����
    public void RestorePreviousBGM()
    {
        if (previousBGM != null)
        {
            PlayBGM(previousBGM);
        }
    }
}