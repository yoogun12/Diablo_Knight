using UnityEngine;
using TMPro;  // TextMeshPro를 쓸 때 필요

public class ShopManager : MonoBehaviour
{
    private int hpLevel;
    private int speedLevel;

    public TextMeshProUGUI hpCostText;     // 최대체력 업그레이드 비용 텍스트
    public TextMeshProUGUI speedCostText;  // 이동속도 업그레이드 비용 텍스트

    private void Start()
    {
        hpLevel = PlayerPrefs.GetInt("HP_LEVEL", 0);
        speedLevel = PlayerPrefs.GetInt("SPEED_LEVEL", 0);

        UpdateCostTexts();  // ← 이 줄을 추가하면 시작하자마자 가격이 뜸!
    }

    public void UpgradeMaxHealth()
    {
        int cost = GetUpgradeCost(hpLevel);

        if (GameDataManager.Instance.playerData.totalCoins >= cost)
        {
            GameDataManager.Instance.playerData.totalCoins -= cost;
            GameDataManager.Instance.SaveData(GameDataManager.Instance.playerData);

            hpLevel++;
            PlayerPrefs.SetInt("HP_LEVEL", hpLevel);

            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.maxHealth += 20;
                playerHealth.currentHealth = playerHealth.maxHealth;

                if (playerHealth.hpSlider != null)
                {
                    playerHealth.hpSlider.maxValue = playerHealth.maxHealth;
                    playerHealth.hpSlider.value = playerHealth.currentHealth;
                }
            }

            UpdateCostTexts();

            // 여기서 코인 텍스트 갱신 호출
            ShopUiManager shopUiManager = FindObjectOfType<ShopUiManager>();
            if (shopUiManager != null)
                shopUiManager.RefreshCoinText();
        }
    }

    public void UpgradeMoveSpeed()
    {
        int cost = GetUpgradeCost(speedLevel);

        if (GameDataManager.Instance.playerData.totalCoins >= cost)
        {
            GameDataManager.Instance.playerData.totalCoins -= cost;
            GameDataManager.Instance.SaveData(GameDataManager.Instance.playerData);

            speedLevel++;
            PlayerPrefs.SetInt("SPEED_LEVEL", speedLevel);

            Player player = FindObjectOfType<Player>();
            if (player != null)
            {
                var moveSpeedField = typeof(Player).GetField("moveSpeed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                float currentSpeed = (float)moveSpeedField.GetValue(player);
                moveSpeedField.SetValue(player, currentSpeed + 0.5f);
            }

            UpdateCostTexts();

            // 코인 텍스트 갱신 호출
            ShopUiManager shopUiManager = FindObjectOfType<ShopUiManager>();
            if (shopUiManager != null)
                shopUiManager.RefreshCoinText();
        }
    }

    private int GetUpgradeCost(int level)
    {
        return Mathf.RoundToInt(100 * Mathf.Pow(1.5f, level));
    }
    private void UpdateCostTexts()
    {
        if (hpCostText != null)
            hpCostText.text = $"Cost: {GetUpgradeCost(hpLevel)}";

        if (speedCostText != null)
            speedCostText.text = $"Cost: {GetUpgradeCost(speedLevel)}";
    }
}