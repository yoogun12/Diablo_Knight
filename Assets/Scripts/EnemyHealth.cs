using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    private Enemy enemy; // Enemy 참조

    private void Awake()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>(); // Enemy 컴포넌트 가져오기
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy HP: " + currentHealth);

        if (enemy != null)
        {
            enemy.HitFlash(); //  피격 이펙트 호출
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (enemy != null)
        {
            enemy.Die(); // 애니메이션 + 코인 드롭 포함
        }
        else
        {
            Destroy(gameObject); // 안전 제거
        }
    }
}
