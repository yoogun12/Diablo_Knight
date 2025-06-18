using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float totalTime = 600f; // 10분 = 600초
    private float currentTime = 0f;

    public TextMeshProUGUI timerText;

    private bool isTimerRunning = true;

    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("TimerText가 할당되지 않았습니다.");
            enabled = false;
            return;
        }

        currentTime = 0f;
        UpdateTimerUI();
    }

    void Update()
    {
        if (!isTimerRunning) return;

        currentTime += Time.deltaTime;

        if (currentTime >= totalTime)
        {
            currentTime = totalTime;
            isTimerRunning = false;
            TimerEnded();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        Debug.Log("10분 버티기 완료! 클리어 씬으로 이동합니다.");
        SceneManager.LoadScene("GameClear");  // 클리어 씬 이름에 맞게 수정하세요
    }
}