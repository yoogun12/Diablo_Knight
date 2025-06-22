using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldBossHealth : MonoBehaviour
{
    public int maxHealth = 500;
    private int currentHealth;
    private Boss boss;

    void Awake()
    {
        currentHealth = maxHealth;
        boss = GetComponent<Boss>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        boss?.HitFlash();

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("GameClear");
        }
    }
}