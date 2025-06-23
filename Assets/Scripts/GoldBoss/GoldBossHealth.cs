using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldBossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    private int currentHealth;
    private GoldBoss goldboss;

    // ������ �ؽ�Ʈ ������ �Ҵ�
    public GameObject damageTextPrefab;

    void Awake()
    {
        currentHealth = maxHealth;
        goldboss = GetComponent<GoldBoss>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (goldboss != null)
            goldboss.HitFlash();

        ShowDamageText(damage);

        if (currentHealth <= 0)
        {
            // 1. ���� ����
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.OnGameClear(); // sessionCoinScore �����
            }
            else
            {
                Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�.");
            }

            // 2. ���� Ŭ���� �� ��ȯ
            SceneManager.LoadScene("GameClear");
        }
    }

    private void ShowDamageText(int damage)
    {
        if (damageTextPrefab == null) return;

        GameObject canvasObj = GameObject.Find("WorldCanvas");
        if (canvasObj == null) return;

        Canvas canvas = canvasObj.GetComponent<Canvas>();
        if (canvas == null) return;

        // ���� ��ġ���� �ణ �������� ������ �ؽ�Ʈ ��ġ ����
        Vector3 worldPos = transform.position + new Vector3(0.5f, 1.5f, 0f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        GameObject textObj = Instantiate(damageTextPrefab, canvas.transform);
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.localPosition = localPoint;

        DamageText dmgText = textObj.GetComponent<DamageText>();
        if (dmgText != null)
            dmgText.SetText(damage);
    }
}