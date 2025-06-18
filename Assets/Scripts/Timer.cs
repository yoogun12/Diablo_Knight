using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float totalTime = 600f; // 10�� = 600��
    private float currentTime = 0f;

    public TextMeshProUGUI timerText;

    private bool isTimerRunning = true;

    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("TimerText�� �Ҵ���� �ʾҽ��ϴ�.");
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
        Debug.Log("10�� ��Ƽ�� �Ϸ�! Ŭ���� ������ �̵��մϴ�.");
        SceneManager.LoadScene("GameClear");  // Ŭ���� �� �̸��� �°� �����ϼ���
    }
}