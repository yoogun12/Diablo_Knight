using UnityEngine;

public class DarkBall : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;
    public int damage = 20;

    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
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
            Destroy(gameObject); // ÃÑ¾Ë ÆÄ±«
        }
        else if (collision.CompareTag("Obstacle"))
        {
            Destroy(gameObject); // º® µî¿¡ ¸ÂÀ¸¸é ÃÑ¾Ë ÆÄ±«
        }
    }
}