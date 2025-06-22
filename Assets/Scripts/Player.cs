using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    float moveSpeed = 3f;
    Vector2 input;
    Vector2 velocity;
    Vector2 lastInput = Vector2.right;

    [Header("Shooting")]
    public int bulletCount = 1;
    public float damage = 20f;
    public float bulletSpeed = 10f;
    private int baseBulletCount = 1;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;

    [Header("Coin & UI")]
    private int sessionCoinScore = 0;
    public TextMeshProUGUI uiCoin;
    public TextMeshProUGUI powerUpTimerText;

    [Header("Power Up")]
    private float powerUpDuration = 10f;
    private Dictionary<PowerUpItem.PowerUpType, Coroutine> activePowerUps = new();
    private Dictionary<PowerUpItem.PowerUpType, float> remainingTimes = new();

    Rigidbody2D rb;
    Animator ani;
    SpriteRenderer spriter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        velocity = input * moveSpeed;

        if (input != Vector2.zero)
            lastInput = input;

        ani.SetFloat("Speed", input.sqrMagnitude);

        if (input.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = input.x > 0 ? 0.08f : -0.08f;
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        Vector2 shootDir = input == Vector2.zero ? lastInput : input;

        if (bulletCount == 1)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            DarkBall darkBall = bullet.GetComponent<DarkBall>();
            darkBall.Initialize(shootDir.normalized, bulletSpeed, Mathf.RoundToInt(damage));
        }
        else
        {
            float angleStep = 30f;
            float startAngle = -angleStep * (bulletCount - 1) / 2f;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector2 rotatedDir = Quaternion.Euler(0, 0, angle) * shootDir;

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                DarkBall darkBall = bullet.GetComponent<DarkBall>();
                darkBall.Initialize(rotatedDir.normalized, bulletSpeed, Mathf.RoundToInt(damage));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            CoinItem coin = collision.GetComponent<CoinItem>();
            if (coin == null)
            {
                Debug.LogWarning("CoinItem 컴포넌트가 없습니다.");
                return;
            }

            sessionCoinScore += coin.GetCoin();

            if (uiCoin != null)
                uiCoin.text = sessionCoinScore.ToString();
            else
                Debug.LogWarning("uiCoin이 할당되어 있지 않습니다.");

            Destroy(collision.gameObject);
        }
    }

    public void OnGameOver()
    {
        if (GameDataManager.Instance == null || GameDataManager.Instance.playerData == null)
        {
            Debug.LogError("GameDataManager.Instance 또는 playerData가 할당되어 있지 않습니다.");
            return;
        }

        GameDataManager.Instance.playerData.totalCoins += sessionCoinScore;
        GameDataManager.Instance.SaveData(GameDataManager.Instance.playerData);
    }

    public void HitFlash()
    {
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        Color originalColor = spriter.color;
        spriter.color = new Color(1f, 1f, 1f, 0.64f);
        yield return new WaitForSeconds(0.1f);
        spriter.color = originalColor;
    }

    public void ApplyPowerUp(PowerUpItem powerUp)
    {
        switch (powerUp.powerUpType)
        {
            case PowerUpItem.PowerUpType.BulletCount:
                bulletCount += Mathf.RoundToInt(powerUp.powerUpValue);
                break;
            case PowerUpItem.PowerUpType.BulletSpeed:
                bulletSpeed += powerUp.powerUpValue;
                break;
            case PowerUpItem.PowerUpType.Damage:
                damage += powerUp.powerUpValue;
                break;
        }

        if (activePowerUps.ContainsKey(powerUp.powerUpType))
        {
            StopCoroutine(activePowerUps[powerUp.powerUpType]);
        }

        Coroutine routine = StartCoroutine(RemovePowerUpAfterDelay(powerUp));
        activePowerUps[powerUp.powerUpType] = routine;

        Debug.Log($"PowerUp applied: {powerUp.itemName}");
    }

    private IEnumerator RemovePowerUpAfterDelay(PowerUpItem powerUp)
    {
        float timeLeft = powerUpDuration;
        remainingTimes[powerUp.powerUpType] = timeLeft;

        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            remainingTimes[powerUp.powerUpType] = timeLeft;
            UpdatePowerUpUI();
            yield return null;
        }

        switch (powerUp.powerUpType)
        {
            case PowerUpItem.PowerUpType.BulletCount:
                bulletCount = baseBulletCount;
                break;
            case PowerUpItem.PowerUpType.BulletSpeed:
                bulletSpeed = 10f;
                break;
            case PowerUpItem.PowerUpType.Damage:
                damage = 20f;
                break;
        }

        remainingTimes.Remove(powerUp.powerUpType);
        activePowerUps.Remove(powerUp.powerUpType);

        if (remainingTimes.Count == 0)
            ClearPowerUpUI();

        Debug.Log($"{powerUp.itemName} 효과 종료됨");
    }

    private void UpdatePowerUpUI()
    {
        if (powerUpTimerText == null) return;

        string result = "";
        foreach (var kvp in remainingTimes)
        {
            result += $"{kvp.Key}: {kvp.Value:F1}s\n";
        }

        powerUpTimerText.text = result;
    }

    private void ClearPowerUpUI()
    {
        if (powerUpTimerText != null)
            powerUpTimerText.text = "";
    }
}