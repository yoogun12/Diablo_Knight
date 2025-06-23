using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;
    public int contactDamage = 10;
    public GameObject coinPrefab;
    [Range(0f, 1f)]
    public float coinDropChance = 1f;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator ani;

    private float damageInterval = 1.0f;
    private Dictionary<GameObject, float> lastDamageTimeDict = new Dictionary<GameObject, float>();

    // 여러 파워업 프리팹 중 하나를 랜덤 드롭
    public GameObject[] powerUpPrefabs;
    [Range(0f, 1f)] public float powerUpDropChance = 0.2f;

    private bool isDead = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                target = playerObj.GetComponent<Rigidbody2D>();
        }
    }

    private void FixedUpdate()
    {
        if (isDead || target == null) return;

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
                lastDamageTimeDict[collision.gameObject] = Time.time;
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
                    lastDamageTime = 0f;

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
            lastDamageTimeDict.Remove(collision.gameObject);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (ani != null)
        {
            ani.SetTrigger("Die");
        }

        TryDropReward();  // 하나만 드롭

        StartCoroutine(DelayedDestroy());
    }

    private void TryDropReward()
    {
        float rand = Random.value;

        if (rand < coinDropChance)
        {
            if (coinPrefab != null)
                Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
        else if (rand < coinDropChance + powerUpDropChance)
        {
            if (powerUpPrefabs != null && powerUpPrefabs.Length > 0)
            {
                GameObject prefabToDrop = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
                Instantiate(prefabToDrop, transform.position, Quaternion.identity);
            }
        }
        // 아무것도 안 뜨는 경우도 있음
    }

    public void HitFlash()
    {
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        Color originalColor = spriter.color;
        spriter.color = new Color(1f, 1f, 1f, 0.6f);
        yield return new WaitForSeconds(0.1f);
        spriter.color = originalColor;
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}