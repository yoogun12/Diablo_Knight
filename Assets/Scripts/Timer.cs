using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [Header("UI Root")]
    public GameObject BossECan;

    [Header("Timer Settings")]
    public float totalTime = 300f;
    private float currentTime = 0f;
    public TextMeshProUGUI timerText;

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    [Header("Warning UI")]
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI rushText;

    [Header("Red Outline Warning")]
    public GameObject[] redOutlines;
    public TextMeshProUGUI[] dangerTexts;

    [Header("Fade & Camera")]
    public CanvasGroup fadeCanvasGroup;
    public Camera mainCamera;
    public TextMeshProUGUI bossExplainText;
    public float fadeDuration = 1f;
    public float cameraMoveDuration = 1f;
    public float bossExplainDuration = 3f;
    public float zoomInSize = 0.5f;

    private bool bossSpawned = false;
    private bool warningShown = false;
    private bool redOutlineWarningShown = false;
    private bool rushStarted = false;
    private bool bossDefeated = false;

    private Vector3 originalCameraPosition;
    private float originalCameraSize;

    private EnemySpawner spawner;

    void Start()
    {
        currentTime = 0f;
        UpdateTimerUI();

        warningText?.gameObject.SetActive(false);
        rushText?.gameObject.SetActive(false);
        bossExplainText?.gameObject.SetActive(false);
        fadeCanvasGroup.alpha = 0f;

        foreach (var outline in redOutlines)
            outline?.SetActive(false);

        foreach (var txt in dangerTexts)
            txt?.gameObject.SetActive(false);

        originalCameraPosition = mainCamera.transform.position;
        if (mainCamera.orthographic)
            originalCameraSize = mainCamera.orthographicSize;

        spawner = FindObjectOfType<EnemySpawner>();
    }

    void Update()
    {
        if (bossDefeated) return;

        currentTime += Time.deltaTime;

        if (!rushStarted && currentTime >= 180f && currentTime < 195f)
        {
            rushStarted = true;
            StartCoroutine(StartRushPhase());
        }

        if (!warningShown && currentTime >= totalTime - 10f)
        {
            warningShown = true;
            StartCoroutine(ShowWarning());

            if (!redOutlineWarningShown)
            {
                redOutlineWarningShown = true;
                StartCoroutine(ShowRedOutlineWarning());
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && !warningShown)
        {
            currentTime = totalTime - 10f;
            warningShown = true;
            StartCoroutine(ShowWarning());

            if (!redOutlineWarningShown)
            {
                redOutlineWarningShown = true;
                StartCoroutine(ShowRedOutlineWarning());
            }

            Debug.Log("치트키로 4분 50초로 이동");
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (bossSpawned)
        {
            timerText.text = "보스를 잡으십시오!";
        }
        else
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    void SpawnBoss()
    {
        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        StartCoroutine(BossSummonSequence(boss.transform));
        Debug.Log("보스 소환됨!");
    }

    IEnumerator StartRushPhase()
    {
        float originalSwarmerInterval = spawner.swarmerInterval;
        float originalBigSwarmerInterval = spawner.bigSwarmerInterval;

        spawner.swarmerInterval *= 0.25f;
        spawner.bigSwarmerInterval *= 0.25f;

        rushText?.gameObject.SetActive(true);
        yield return new WaitForSeconds(15f);
        rushText?.gameObject.SetActive(false);

        spawner.swarmerInterval = originalSwarmerInterval;
        spawner.bigSwarmerInterval = originalBigSwarmerInterval;
    }

    IEnumerator ShowWarning()
    {
        warningText.gameObject.SetActive(true);
        float duration = 10f;
        float elapsed = 0f;
        Vector3 originalScale = warningText.rectTransform.localScale;
        Color originalColor = warningText.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = 1f + Mathf.PingPong(elapsed * 2f, 0.05f);
            float alpha = 0.5f + Mathf.PingPong(elapsed * 4f, 0.5f);
            warningText.rectTransform.localScale = originalScale * scale;
            Color c = originalColor; c.a = alpha;
            warningText.color = c;
            yield return null;
        }

        warningText.rectTransform.localScale = originalScale;
        warningText.color = originalColor;
        warningText.gameObject.SetActive(false);

        if (!bossSpawned)
        {
            bossSpawned = true;
            SpawnBoss();
        }
    }

    IEnumerator ShowRedOutlineWarning()
    {
        foreach (var o in redOutlines) o?.SetActive(true);
        foreach (var t in dangerTexts) t?.gameObject.SetActive(true);

        float duration = 10f;
        float elapsed = 0f;
        Color[] originalColors = new Color[dangerTexts.Length];
        for (int i = 0; i < dangerTexts.Length; i++)
            originalColors[i] = dangerTexts[i].color;

        CanvasGroup[] outlineGroups = new CanvasGroup[redOutlines.Length];
        for (int i = 0; i < redOutlines.Length; i++)
        {
            if (redOutlines[i] == null) continue;
            var cg = redOutlines[i].GetComponent<CanvasGroup>() ?? redOutlines[i].AddComponent<CanvasGroup>();
            outlineGroups[i] = cg;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = 0.5f + Mathf.PingPong(elapsed * 4f, 0.5f);

            foreach (var cg in outlineGroups)
                if (cg != null) cg.alpha = alpha;

            for (int i = 0; i < dangerTexts.Length; i++)
            {
                if (dangerTexts[i] != null)
                {
                    Color c = originalColors[i];
                    c.a = alpha;
                    dangerTexts[i].color = c;
                }
            }

            yield return null;
        }

        foreach (var cg in outlineGroups)
            if (cg != null) cg.alpha = 1f;

        foreach (var o in redOutlines) o?.SetActive(false);
        foreach (var t in dangerTexts) t?.gameObject.SetActive(false);
    }

    public void OnBossDefeated(Transform bossTransform)
    {
        if (!bossDefeated)
            StartCoroutine(BossDeathSequence(bossTransform));
    }

    IEnumerator BossSummonSequence(Transform bossTransform)
    {
        Time.timeScale = 0f;

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        originalCameraPosition = mainCamera.transform.position;
        originalCameraSize = mainCamera.orthographicSize;

        Vector3 targetPos = new Vector3(bossTransform.position.x, bossTransform.position.y, originalCameraPosition.z);
        mainCamera.transform.position = targetPos;
        mainCamera.orthographicSize = originalCameraSize;

        BossECan?.SetActive(false);

        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        float elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            mainCamera.orthographicSize = Mathf.Lerp(originalCameraSize, zoomInSize, t);
            yield return null;
        }

        bossExplainText?.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(bossExplainDuration);
        bossExplainText?.gameObject.SetActive(false);

        elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            mainCamera.orthographicSize = Mathf.Lerp(zoomInSize, originalCameraSize, t);
            yield return null;
        }

        BossECan?.SetActive(true);

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));
        mainCamera.transform.position = originalCameraPosition;
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        Time.timeScale = 1f;
    }

    IEnumerator BossDeathSequence(Transform bossTransform)
    {
        bossDefeated = true;
        Time.timeScale = 0f;

        BossECan?.SetActive(false);

        originalCameraPosition = mainCamera.transform.position;
        originalCameraSize = mainCamera.orthographicSize;

        Vector3 bossPos = new Vector3(bossTransform.position.x, bossTransform.position.y, originalCameraPosition.z);
        mainCamera.transform.position = bossPos;

        SpriteRenderer sr = bossTransform.GetComponent<SpriteRenderer>();
        float elapsed = 0f;

        while (elapsed < 2f)
        {
            elapsed += Time.unscaledDeltaTime;
            if (sr != null)
            {
                Color c = sr.color;
                c.a = Mathf.Lerp(1f, 0f, elapsed / 2f);
                sr.color = c;
            }
            yield return null;
        }

        Destroy(bossTransform.gameObject);

        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameClear");
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            fadeCanvasGroup.alpha = alpha;
            yield return null;
        }
        fadeCanvasGroup.alpha = endAlpha;
    }

    public void BossDie(Transform bossTransform)
    {
        if (bossTransform != null)
        {
            OnBossDefeated(bossTransform);
        }
        else
        {
            Debug.LogWarning("BossDie() 호출 시 bossTransform이 null입니다.");
        }
    }

}