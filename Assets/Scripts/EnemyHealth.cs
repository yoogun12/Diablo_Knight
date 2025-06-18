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
        // Enemy ��ũ��Ʈ ��������
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Die();  // ���� ��� ������ Die ȣ��
        }
        else
        {
            Destroy(gameObject); // �����ϰ� �׳� ����
        }
    }
}
