using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public ShopUiManager shopUiManager;  // ���� �޴��� ������ ����
    public ShopManager shopManager;      // ������ ����

    public void ResetGameProgress()
    {
        Debug.Log("���� �޴����� ���� ���� �ʱ�ȭ");

        // PlayerPrefs �ʱ�ȭ
        PlayerPrefs.SetInt("HP_LEVEL", 0);
        PlayerPrefs.SetInt("SPEED_LEVEL", 0);
        PlayerPrefs.Save();

        // GameDataManager ���� ���� ���� �ʱ�ȭ
        if (GameDataManager.Instance != null && GameDataManager.Instance.playerData != null)
        {
            GameDataManager.Instance.playerData.totalCoins = 0;
            GameDataManager.Instance.SaveData(GameDataManager.Instance.playerData);
        }

        // UI ����
        if (shopUiManager != null)
        {
            shopUiManager.RefreshCoinText();
        }

        if (shopManager != null)
        {
            shopManager.SendMessage("Start");
        }
    }
}