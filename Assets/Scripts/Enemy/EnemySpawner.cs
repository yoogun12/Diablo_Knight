using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject swarmerPrefab;
    [SerializeField] private GameObject bigSwarmerPrefab;

    [SerializeField] private float swarmerInterval = 3.5f;
    [SerializeField] private float bigSwarmerInterval = 10f;

    [SerializeField] private float spawnRadiusMin = 4f;   // �÷��̾�κ��� �ּ� �Ÿ�
    [SerializeField] private float spawnRadiusMax = 8f;   // �ִ� �Ÿ�

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            StartCoroutine(spawnEnemy(swarmerInterval, swarmerPrefab));
            StartCoroutine(spawnEnemy(bigSwarmerInterval, bigSwarmerPrefab));
        }
        else
        {
            Debug.LogWarning("Player�� ã�� �� �����ϴ�. ��ȯ �Ұ�.");
        }
    }

    private IEnumerator spawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);

        if (player != null)
        {
            Vector2 spawnPos = GetValidSpawnPosition();
            Instantiate(enemy, spawnPos, Quaternion.identity);
        }

        StartCoroutine(spawnEnemy(interval, enemy));
    }

    private Vector2 GetValidSpawnPosition()
    {
        // ���� ���� * �Ÿ�
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(spawnRadiusMin, spawnRadiusMax);
        Vector2 spawnPos = (Vector2)player.position + randomDir * randomDistance;

        return spawnPos;
    }
}