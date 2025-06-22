using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float totalTime = 600f; // 10��
    private float currentTime = 0f;
    public TextMeshProUGUI timerText;

    public GameObject bossPrefab; // ���� ������
    public Transform bossSpawnPoint; // ���� ��ȯ ��ġ

    private bool bossSpawned = false;

    void Start()
    {
        currentTime = 0f;
        UpdateTimerUI();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        // ġƮŰ: T Ű�� ������ ��� 10�� ���� �� ���� ��ȯ
        if (Input.GetKeyDown(KeyCode.T) && !bossSpawned)
        {
            currentTime = totalTime;
            bossSpawned = true;
            SpawnBoss();
            Debug.Log("ġƮŰ�� ������ ��ȯ�Ǿ����ϴ�.");
        }

        if (!bossSpawned && currentTime >= totalTime)
        {
            bossSpawned = true;
            SpawnBoss();
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        Debug.Log("���� ���� ��ȯ��!");
    }
}