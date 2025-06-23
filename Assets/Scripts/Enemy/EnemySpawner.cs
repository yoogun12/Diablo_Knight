using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;  // 텍스트 UI 사용

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;

    [SerializeField] public float swarmerInterval = 3.5f;
    [SerializeField] public float bigSwarmerInterval = 10f;

    [SerializeField] private float spawnRadiusMin = 4f;   // 플레이어로부터 최소 거리
    [SerializeField] private float spawnRadiusMax = 8f;   // 최대 거리

    [SerializeField] private Tilemap No;      // "No" 타일맵 참조

    [Header("Warning Text UI")]
    [SerializeField] private TextMeshProUGUI warningTextUI;
    [SerializeField] private float warningTextDuration = 3f;

    private Transform player;

    private bool isSpawnBoostActive = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            StartCoroutine(spawnEnemy(swarmerInterval, swarmerPrefab));
            StartCoroutine(spawnEnemy(bigSwarmerInterval, bigSwarmerPrefab));
            StartCoroutine(CheckSpawnBoost());
        }
        else
        {
            Debug.LogWarning("Player를 찾을 수 없습니다. 소환 불가.");
        }

        if (warningTextUI != null)
            warningTextUI.gameObject.SetActive(false);
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        while (true)
        {
            float spawnMultiplier = 1f;
            if (isSpawnBoostActive)
                spawnMultiplier = 2f; // 2배 빠르게 소환

            yield return new WaitForSeconds(interval / spawnMultiplier);

            if (player != null)
            {
                Vector2 spawnPos = GetValidSpawnPosition();
                if (spawnPos != Vector2.zero) // 유효한 위치 찾음
                    Instantiate(enemy, spawnPos, Quaternion.identity);
                else
                    Debug.LogWarning("유효한 몬스터 소환 위치를 찾지 못했습니다.");
            }
        }
    }

    private Vector2 GetValidSpawnPosition()
    {
        const int maxAttempts = 30;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            attempts++;

            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax);
            Vector2 candidatePos = (Vector2)player.position + randomDir * randomDistance;

            Vector3Int tileCell = No.WorldToCell(candidatePos);
            TileBase tile = No.GetTile(tileCell);

            if (tile == null)
            {
                return candidatePos;
            }
        }

        return Vector2.zero;
    }

    private IEnumerator CheckSpawnBoost()
    {
        while (true)
        {
            float time = Time.timeSinceLevelLoad;

            if (!isSpawnBoostActive && time >= 180f && time < 195f) // 3분부터 15초간
            {
                isSpawnBoostActive = true;

                if (warningTextUI != null)
                {
                    warningTextUI.text = "15초 동안 몬스터가 급증합니다!";
                    warningTextUI.gameObject.SetActive(true);
                    StartCoroutine(HideWarningTextAfterDelay(warningTextDuration));
                }
            }
            else if (time >= 195f && isSpawnBoostActive)
            {
                isSpawnBoostActive = false;
            }

            yield return null;
        }
    }

    private IEnumerator HideWarningTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (warningTextUI != null)
            warningTextUI.gameObject.SetActive(false);
    }
}