using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ ���� �߰�

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Player player; // Player ��ũ��Ʈ ���� ����

    private void Awake()
    {
        currentHealth = maxHealth;
        player = GetComponent<Player>(); // ���� ���ӿ�����Ʈ�� Player ������Ʈ�� �ִٰ� ����
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            if (player != null)
            {
                player.OnGameOver();  // ȹ�� ���� ���� ó��
            }
            else
            {
                Debug.LogWarning("Player ������Ʈ�� ã�� ���߽��ϴ�.");
            }

            SceneManager.LoadScene("GameOver");  // ���ӿ��� ������ ��ȯ
        }
    }
}