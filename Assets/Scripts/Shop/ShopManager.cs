using UnityEngine;
using TMPro;  // TextMeshPro�� �� �� �ʿ�

public class ShopManager : MonoBehaviour
{
    private int hpLevel;
    private int speedLevel;

    public TextMeshProUGUI hpCostText;     // �ִ�ü�� ���׷��̵� ��� �ؽ�Ʈ
    public TextMeshProUGUI speedCostText;  // �̵��ӵ� ���׷��̵� ��� �ؽ�Ʈ

    private void Start()
    {
        hpLevel = PlayerPrefs.GetInt("HP_LEVEL", 0);
        speedLevel = PlayerPrefs.GetInt("SPEED_LEVEL", 0);

        UpdateCostTexts();  // �� �� ���� �߰��ϸ� �������ڸ��� ������ ��!
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

            // ���⼭ ���� �ؽ�Ʈ ���� ȣ��
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

            // ���� �ؽ�Ʈ ���� ȣ��
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