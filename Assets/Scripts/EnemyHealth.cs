using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    private bool isDead = false; // << 추가됨

    private Enemy enemy;

    public GameObject damageTextPrefab;
    private void Awake()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // << 이미 죽었으면 아무 처리도 안 함

        currentHealth -= damage;
        ShowDamageText(damage);
        Debug.Log("Enemy HP: " + currentHealth);

        if (enemy != null)
        {
            enemy.HitFlash();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; // 혹시 몰라서 한 번 더 방어
        isDead = true;      // << 여기서 딱 한 번만 죽게 설정

        if (enemy != null)
        {
            enemy.Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ShowDamageText(int damage)
    {
        if (damageTextPrefab == null) return;

        GameObject canvasObj = GameObject.Find("WorldCanvas");
        if (canvasObj == null) return;

        Canvas canvas = canvasObj.GetComponent<Canvas>();
        if (canvas == null) return;

        // 적의 중심에서 오른쪽 위로 약간 이동
        Vector3 worldPos = transform.position + new Vector3(0.5f, 0.1f, 0f);
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