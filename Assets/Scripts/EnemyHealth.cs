using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Enemy 스크립트 가져오기
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Die();  // 코인 드롭 포함한 Die 호출
        }
        else
        {
            Destroy(gameObject); // 안전하게 그냥 제거
        }
    }
}
