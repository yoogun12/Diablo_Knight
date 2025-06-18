using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    float moveSpeed = 3f;

    Rigidbody2D rb;
    Animator ani;

    public Vector2 input;
    Vector2 velocity;

    Vector2 lastInput = Vector2.right; // 기본 오른쪽 방향

    [SerializeField]
    public GameObject bulletPrefab; // 총알 프리팹
    [SerializeField]
    public Transform firePoint;     // 총알 발사 위치

    // 이번 플레이에서 획득한 코인 점수
    private int sessionCoinScore = 0;

    public TextMeshProUGUI uiCoin;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input = input.normalized;
        velocity = input * moveSpeed;

        if (input != Vector2.zero)
        {
            lastInput = input;  // 마지막 입력 방향 갱신
        }

        ani.SetFloat("Speed", input.sqrMagnitude);

        if (input.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = input.x > 0 ? 0.08f : -0.08f;
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(KeyCode.Space))  // 스페이스 키 발사
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        Vector2 shootDir = input == Vector2.zero ? lastInput : input;

        bullet.GetComponent<DarkBall>().Initialize(shootDir.normalized);
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
                Debug.LogWarning("CoinItem 컴포넌트가 없습니다.");
                return;
            }

            sessionCoinScore += coin.GetCoin();

            if (uiCoin != null)
            {
                uiCoin.text = sessionCoinScore.ToString();
            }
            else
            {
                Debug.LogWarning("uiCoin이 할당되어 있지 않습니다.");
            }

            Destroy(collision.gameObject);
        }
    }

    // 게임 종료 시 호출해서 세션 코인을 누적 코인에 더하고 저장하세요
    public void OnGameOver()
    {
        if (GameDataManager.Instance == null)
        {
            Debug.LogError("GameDataManager.Instance가 할당되어 있지 않습니다.");
            return;
        }

        if (GameDataManager.Instance.playerData == null)
        {
            Debug.LogError("playerData가 초기화되지 않았습니다.");
            return;
        }

        GameDataManager.Instance.playerData.totalCoins += sessionCoinScore;
        GameDataManager.Instance.SaveData(GameDataManager.Instance.playerData);
    }
}