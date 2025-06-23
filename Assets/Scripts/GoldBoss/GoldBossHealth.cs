using UnityEngine;

public class GoldBossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    private int currentHealth;
    private GoldBoss goldboss;
    public GameObject damageTextPrefab;

    public Timer timer; // 연결 필요

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

    private void ShowDamageText(int damage) { /* 생략 */ }
}
