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

    Vector2 lastInput = Vector2.right; // �⺻ ������ ����

    [SerializeField]
    public GameObject bulletPrefab; // �Ѿ� ������
    [SerializeField]
    public Transform firePoint;     // �Ѿ� �߻� ��ġ

    public int coin;

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
            lastInput = input;  // ������ �Է� ���� ����
        }

        ani.SetFloat("Speed", input.sqrMagnitude);

        if (input.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = input.x > 0 ? 0.08f : -0.08f;
            transform.localScale = scale;
        }

        if (Input.GetKeyDown(KeyCode.Space))  // �����̽� Ű �߻�
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
        
        if(collision.CompareTag("Coin"))
        {
            coin += collision.GetComponent<CoinItem>().GetCoin();
            Destroy(collision.gameObject);
            uiCoin.text = "coin : " + coin.ToString();
        }
    }
}
