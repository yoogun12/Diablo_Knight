using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환 위해 추가

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    private Player player; // Player 스크립트 참조 변수

    private void Awake()
    {
        currentHealth = maxHealth;
        player = GetComponent<Player>(); // 같은 게임오브젝트에 Player 컴포넌트가 있다고 가정
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
                player.OnGameOver();  // 획득 코인 저장 처리
            }
            else
            {
                Debug.LogWarning("Player 컴포넌트를 찾지 못했습니다.");
            }

            SceneManager.LoadScene("GameOver");  // 게임오버 씬으로 전환
        }
    }
}