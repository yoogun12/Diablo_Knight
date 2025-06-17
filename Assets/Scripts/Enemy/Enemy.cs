using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    public int contactDamage = 10; // �÷��̾� ���� �� ������

    public GameObject coinPrefab; // ����� ���� ������
    [Range(0f, 1f)]
    public float coinDropChance = 0.3f; // ���� ��� Ȯ�� (��: 30%)

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

        // �ִϸ��̼� �ӵ� ����
        ani.SetFloat("Speed", dirVec.sqrMagnitude);

        // �¿� ����
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

    // �� �޼��带 �ܺο��� ȣ���ؼ� ���� ���� �� ����
    public void Die()
    {
        TryDropCoin();
        Destroy(gameObject); // �� ������Ʈ ����
    }

    private void TryDropCoin()
    {
        if (coinPrefab != null && Random.value < coinDropChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}
