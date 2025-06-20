using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;

    private Enemy enemy; // Enemy ����

    private void Awake()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>(); // Enemy ������Ʈ ��������
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy HP: " + currentHealth);

        if (enemy != null)
        {
            enemy.HitFlash(); //  �ǰ� ����Ʈ ȣ��
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
            enemy.Die(); // �ִϸ��̼� + ���� ��� ����
        }
        else
        {
            Destroy(gameObject); // ���� ����
        }
    }
}
