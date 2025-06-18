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
    public float coinDropChance = 1f; // 코인 드롭 확률 (예: 30%)

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator ani;

    // 지속 데미지용 시간 체크 변수
    private float damageInterval = 1.0f; // 1초마다 데미지
    private Dictionary<GameObject, float> lastDamageTimeDict = new Dictionary<GameObject, float>();

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

        ani.SetFloat("Speed", dirVec.sqrMagnitude);

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
                lastDamageTimeDict[collision.gameObject] = Time.time; // 데미지 준 시간 기록
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                float lastDamageTime;
                if (!lastDamageTimeDict.TryGetValue(collision.gameObject, out lastDamageTime))
                {
                    lastDamageTime = 0f;
                }

                if (Time.time - lastDamageTime >= damageInterval)
                {
                    playerHealth.TakeDamage(contactDamage);
                    lastDamageTimeDict[collision.gameObject] = Time.time;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (lastDamageTimeDict.ContainsKey(collision.gameObject))
        {
            lastDamageTimeDict.Remove(collision.gameObject);
        }
    }

    public void Die()
    {
        TryDropCoin();
        Destroy(gameObject);
    }

    private void TryDropCoin()
    {
        Debug.Log("TryDropCoin called");
        if (coinPrefab != null)
        {
            Debug.Log("coinPrefab is assigned");
            if (Random.value < coinDropChance)
            {
                Debug.Log("Coin drop success");
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("Coin drop failed by chance");
            }
        }
        else
        {
            Debug.LogWarning("coinPrefab is null!");
        }
    }
}
