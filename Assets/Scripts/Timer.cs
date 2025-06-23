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

    private Vector3 originalCameraPosition;
    private float originalCameraSize;

    void Start()
    {
        currentTime = 0f;
        UpdateTimerUI();

        if (warningText != null)
            warningText.gameObject.SetActive(false);

        foreach (var outline in redOutlines)
            if (outline != null) outline.SetActive(false);

        foreach (var txt in dangerTexts)
            if (txt != null) txt.gameObject.SetActive(false);

        if (bossExplainText != null)
            bossExplainText.gameObject.SetActive(false);

        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0f;

        originalCameraPosition = mainCamera.transform.position;
        if (mainCamera.orthographic)
            originalCameraSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        currentTime += Time.deltaTime;

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

            Debug.Log("ġƮŰ�� 4�� 50�ʷ� �̵�");
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (bossSpawned)
        {
            timerText.text = "������ �����ʽÿ�!";
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
        Debug.Log("���� ��ȯ��!");
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
        foreach (var o in redOutlines) if (o != null) o.SetActive(true);
        foreach (var t in dangerTexts) if (t != null) t.gameObject.SetActive(true);

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

        foreach (var o in redOutlines) if (o != null) o.SetActive(false);
        foreach (var t in dangerTexts) if (t != null) t.gameObject.SetActive(false);
    }

    IEnumerator BossSummonSequence(Transform bossTransform)
    {
        Time.timeScale = 0f;

        // 1. ���̵� �ƿ�
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // 2. ī�޶� ���� ��ġ�� �̵� (���� ���� ���� ����)
        originalCameraPosition = mainCamera.transform.position;
        originalCameraSize = mainCamera.orthographicSize;

        Vector3 targetPos = new Vector3(bossTransform.position.x, bossTransform.position.y, originalCameraPosition.z);
        mainCamera.transform.position = targetPos;
        mainCamera.orthographicSize = originalCameraSize;

        // ���� ���� ���� ���� (���� ����)
        if (BossECan != null)
            BossECan.SetActive(false);

        // 3. ���̵� �� (���� �� ���·� ���� ������)
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

        // 4. ���� ���� �� ī�޶� ����
        float elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            mainCamera.orthographicSize = Mathf.Lerp(originalCameraSize, zoomInSize, t);
            yield return null;
        }

  


        // 5. ���� ����
        if (bossExplainText != null)
        {
            bossExplainText.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(bossExplainDuration);
            bossExplainText.gameObject.SetActive(false);
        }

        // 6. ���� ������ �ٽ� �� �ƿ�
        elapsed = 0f;
        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            mainCamera.orthographicSize = Mathf.Lerp(zoomInSize, originalCameraSize, t);
            yield return null;
        }


        // ���� ���� �� UI �ٽ� �ѱ�
        if (BossECan != null)
            BossECan.SetActive(true);

        // 7. �ٽ� ���̵� �ƿ�
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        // 8. ī�޶� ���� ��ġ�� ����
        mainCamera.transform.position = originalCameraPosition;

        // 9. ���̵� �� �� ���� �簳
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
        Time.timeScale = 1f;
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            if (fadeCanvasGroup != null)
                fadeCanvasGroup.alpha = alpha;
            yield return null;
        }
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = endAlpha;
    }
}