using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    private bool isDead = false; // << �߰���

    private Enemy enemy;

    public GameObject damageTextPrefab;
    private void Awake()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // << �̹� �׾����� �ƹ� ó���� �� ��

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
        if (isDead) return; // Ȥ�� ���� �� �� �� ���
        isDead = true;      // << ���⼭ �� �� ���� �װ� ����

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

        // ���� �߽ɿ��� ������ ���� �ణ �̵�
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