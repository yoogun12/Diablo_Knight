using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldBossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    private int currentHealth;
    private GoldBoss goldboss;

    // 데미지 텍스트 프리팹 할당
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
            // 1. 코인 저장
            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.OnGameClear(); // sessionCoinScore 저장됨
            }
            else
            {
                Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다.");
            }

            // 2. 게임 클리어 씬 전환
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

        // 보스 위치에서 약간 위쪽으로 데미지 텍스트 위치 조정
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