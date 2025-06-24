using UnityEngine;

public class GoldBossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    private int currentHealth;
    private GoldBoss goldboss;
    public GameObject damageTextPrefab;

    public Timer timer; // ���� �ʿ�

    void Awake()
    {
        currentHealth = maxHealth;
        goldboss = GetComponent<GoldBoss>();
        if (timer == null) timer = FindObjectOfType<Timer>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (goldboss != null) goldboss.HitFlash();
        ShowDamageText(damage);

        if (currentHealth <= 0)
        {
            Player player = FindObjectOfType<Player>();
            if (player != null) player.OnGameClear();
            if (timer != null) timer.BossDie(this.transform);
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
