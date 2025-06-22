using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider hpSlider;         // HP UI 슬라이더
    public Transform hpUIFollow;    // 슬라이더가 따라다닐 위치 (예: 머리 위 빈 오브젝트)

    private Player player;

    private void Awake()
    {
        currentHealth = maxHealth;
        player = GetComponent<Player>();

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHealth;
            hpSlider.value = currentHealth;
        }
        else
        {
            Debug.LogWarning("hpSlider가 할당되지 않았습니다. UI가 보이지 않을 수 있습니다.");
        }
    }

    private void Update()
    {
        // 슬라이더가 플레이어 위에 따라오게 하되 화면 밖이면 숨김 처리
        if (hpSlider != null && hpUIFollow != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(hpUIFollow.position);
            bool isVisible = screenPos.z > 0 && screenPos.x > 0 && screenPos.x < Screen.width && screenPos.y > 0 && screenPos.y < Screen.height;

            hpSlider.gameObject.SetActive(isVisible);
            if (isVisible)
            {
                hpSlider.transform.position = screenPos;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player HP: " + currentHealth);

        if (player != null)
        {
            player.HitFlash();
        }

        if (hpSlider != null)
        {
            hpSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            if (player != null)
            {
                player.OnGameOver();
            }
            else
            {
                Debug.LogWarning("Player 컴포넌트를 찾지 못했습니다.");
            }

            SceneManager.LoadScene("GameOver");
        }
    }
}