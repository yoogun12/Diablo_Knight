using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Slider hpSlider;         // HP UI �����̴�
    public Transform hpUIFollow;    // �����̴��� ����ٴ� ��ġ (��: �Ӹ� �� �� ������Ʈ)

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
            Debug.LogWarning("hpSlider�� �Ҵ���� �ʾҽ��ϴ�. UI�� ������ ���� �� �ֽ��ϴ�.");
        }
    }

    private void Update()
    {
        // �����̴��� �÷��̾� ���� ������� �ϵ� ȭ�� ���̸� ���� ó��
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
                Debug.LogWarning("Player ������Ʈ�� ã�� ���߽��ϴ�.");
            }

            SceneManager.LoadScene("GameOver");
        }
    }
}