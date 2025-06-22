using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed = 1.5f; // 일반 적보다 느리게
    public int contactDamage = 30;
    public Rigidbody2D target;

    private Rigidbody2D rb;
    private Animator ani;
    private SpriteRenderer spriter;

    private float damageInterval = 1.5f;
    private Dictionary<GameObject, float> lastDamageTimeDict = new Dictionary<GameObject, float>();

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();

        if (target == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                target = playerObj.GetComponent<Rigidbody2D>();
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;

        ani?.SetFloat("Speed", dirVec.sqrMagnitude);

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
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(contactDamage);
                lastDamageTimeDict[collision.gameObject] = Time.time;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                float lastTime;
                if (!lastDamageTimeDict.TryGetValue(collision.gameObject, out lastTime))
                    lastTime = 0f;

                if (Time.time - lastTime >= damageInterval)
                {
                    player.TakeDamage(contactDamage);
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

    // 피격 이펙트 (선택 사항)
    public void HitFlash()
    {
        StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        Color originalColor = spriter.color;
        spriter.color = new Color(1f, 0.5f, 0.5f, 0.7f); // 빨간 느낌
        yield return new WaitForSeconds(0.1f);
        spriter.color = originalColor;
    }
}