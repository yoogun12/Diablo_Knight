using UnityEngine;

public class DarkBall : MonoBehaviour
{
    private float speed = 10f;
    private int damage = 20;
    private Vector2 direction;

    /// <summary>
    /// �Ѿ��� �ʱ�ȭ�մϴ�. ����, �ӵ�, �������� �����ϰ� ���� �ð� �� �ı��˴ϴ�.
    /// </summary>
    public void Initialize(Vector2 dir, float newSpeed, int newDamage)
    {
        direction = dir.normalized;
        speed = newSpeed;
        damage = newDamage;
        Destroy(gameObject, 2f);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
      
            GoldBossHealth bossHealth = collision.GetComponent<GoldBossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}