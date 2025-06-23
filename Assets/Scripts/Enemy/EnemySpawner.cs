using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;  // �ؽ�Ʈ UI ���

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;

    [SerializeField] public float swarmerInterval = 3.5f;
    [SerializeField] public float bigSwarmerInterval = 10f;

    [SerializeField] private float spawnRadiusMin = 4f;   // �÷��̾�κ��� �ּ� �Ÿ�
    [SerializeField] private float spawnRadiusMax = 8f;   // �ִ� �Ÿ�

    [SerializeField] private Tilemap No;      // "No" Ÿ�ϸ� ����

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
            Debug.LogWarning("Player�� ã�� �� �����ϴ�. ��ȯ �Ұ�.");
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
                spawnMultiplier = 2f; // 2�� ������ ��ȯ

            yield return new WaitForSeconds(interval / spawnMultiplier);

            if (player != null)
            {
                Vector2 spawnPos = GetValidSpawnPosition();
                if (spawnPos != Vector2.zero) // ��ȿ�� ��ġ ã��
                    Instantiate(enemy, spawnPos, Quaternion.identity);
                else
                    Debug.LogWarning("��ȿ�� ���� ��ȯ ��ġ�� ã�� ���߽��ϴ�.");
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

            if (!isSpawnBoostActive && time >= 180f && time < 195f) // 3�к��� 15�ʰ�
            {
                isSpawnBoostActive = true;

                if (warningTextUI != null)
                {
                    warningTextUI.text = "15�� ���� ���Ͱ� �����մϴ�!";
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