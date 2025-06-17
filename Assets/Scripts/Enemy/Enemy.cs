using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    public int contactDamage = 10; // 플레이어 접촉 시 데미지

    public GameObject coinPrefab; // 드롭할 코인 프리팹
    [Range(0f, 1f)]
    public float coinDropChance = 0.3f; // 코인 드롭 확률 (예: 30%)

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator ani;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                target = playerObj.GetComponent<Rigidbody2D>();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

        // 애니메이션 속도 설정
        ani.SetFloat("Speed", dirVec.sqrMagnitude);

        // 좌우 반전
        if (dirVec.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = dirVec.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(contactDamage);
            }
        }
    }

    // 이 메서드를 외부에서 호출해서 적을 죽일 수 있음
    public void Die()
    {
        TryDropCoin();
        Destroy(gameObject); // 적 오브젝트 제거
    }

    private void TryDropCoin()
    {
        if (coinPrefab != null && Random.value < coinDropChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}
