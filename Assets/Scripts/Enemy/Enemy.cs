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

    public GameObject powerUpPrefab; // PowerUpItemInstance 프리팹
    [Range(0f, 1f)] public float powerUpDropChance = 0.2f; // 파워업 드롭 확률
    public PowerUpItem[] possiblePowerUps; // 에디터에서 등록할 수 있는 아이템 리스트

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
        TryDropCoin();
        TryDropPowerUp();
        Destroy(gameObject);
    }

    private void TryDropCoin()
    {
        if (coinPrefab != null && Random.value < coinDropChance)
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }

    //  피격 효과 메서드
    public void HitFlash()
    {
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        Color originalColor = spriter.color;
        spriter.color = new Color(1f, 1f, 1f, 0.6f); // 반투명 흰색
        yield return new WaitForSeconds(0.1f);
        spriter.color = originalColor;
    }

    private void TryDropPowerUp()
    {
        if (powerUpPrefab == null || possiblePowerUps.Length == 0)
            return;

        if (Random.value < powerUpDropChance)
        {
            PowerUpItem randomItem = possiblePowerUps[Random.Range(0, possiblePowerUps.Length)];
            GameObject drop = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
            PowerUpItemInstance instance = drop.GetComponent<PowerUpItemInstance>();
            instance.itemData = randomItem;
        }
    }
}