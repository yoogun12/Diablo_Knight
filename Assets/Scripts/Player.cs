using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    float moveSpeed = 3f;
    public int bulletCount = 1;

    Rigidbody2D rb;
    Animator ani;
    SpriteRenderer spriter;

    public Vector2 input;
    Vector2 velocity;

    Vector2 lastInput = Vector2.right;

    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public Transform firePoint;

    private int sessionCoinScore = 0;
    public TextMeshProUGUI uiCoin;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>(); // SpriteRenderer �ʱ�ȭ
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

    void Shoot()
    {
        Vector2 shootDir = input == Vector2.zero ? lastInput : input;

        if (bulletCount == 1)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.GetComponent<DarkBall>().Initialize(shootDir.normalized);
        }
        else
        {
            float angleStep = 30f; // ������ ���� ���� (�Ѱ���)
            float startAngle = -angleStep * (bulletCount - 1) / 2f;

            for (int i = 0; i < bulletCount; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector2 dir = Quaternion.Euler(0, 0, angle) * shootDir;
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<DarkBall>().Initialize(dir.normalized);
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            CoinItem coin = collision.GetComponent<CoinItem>();
            if (coin == null)
            {
                Debug.LogWarning("CoinItem ������Ʈ�� �����ϴ�.");
                return;
            }

            sessionCoinScore += coin.GetCoin();

            if (uiCoin != null)
                uiCoin.text = sessionCoinScore.ToString();
            else
                Debug.LogWarning("uiCoin�� �Ҵ�Ǿ� ���� �ʽ��ϴ�.");

            Destroy(collision.gameObject);
        }
    }

    public void OnGameOver()
    {
        if (GameDataManager.Instance == null || GameDataManager.Instance.playerData == null)
        {
            Debug.LogError("GameDataManager.Instance �Ǵ� playerData�� �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
            return;
        }

        GameDataManager.Instance.playerData.totalCoins += sessionCoinScore;
        GameDataManager.Instance.SaveData(GameDataManager.Instance.playerData);
    }

    // �ǰ� ȿ�� �޼���
    public void HitFlash()
    {
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        Color originalColor = spriter.color;
        spriter.color = new Color(1f, 1f, 1f, 0.64f); // ������ ���
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
                bulletPrefab.GetComponent<DarkBall>().speed += powerUp.powerUpValue;
                break;
            case PowerUpItem.PowerUpType.MoveSpeed:
                moveSpeed += powerUp.powerUpValue;
                break;
                // �ʿ��� ��� �߰� ����
        }

        Debug.Log($"PowerUp applied: {powerUp.itemName}");
    }

}