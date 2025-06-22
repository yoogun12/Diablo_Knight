using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float totalTime = 600f; // 10분
    private float currentTime = 0f;
    public TextMeshProUGUI timerText;

    public GameObject bossPrefab; // 보스 프리팹
    public Transform bossSpawnPoint; // 보스 소환 위치

    private bool bossSpawned = false;

    void Start()
    {
        currentTime = 0f;
        UpdateTimerUI();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        // 치트키: T 키를 누르면 즉시 10분 도달 → 보스 소환
        if (Input.GetKeyDown(KeyCode.T) && !bossSpawned)
        {
            currentTime = totalTime;
            bossSpawned = true;
            SpawnBoss();
            Debug.Log("치트키로 보스가 소환되었습니다.");
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
        Debug.Log("보스 몬스터 소환됨!");
    }
}