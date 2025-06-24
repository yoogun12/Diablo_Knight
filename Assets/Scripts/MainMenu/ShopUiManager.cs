using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopUiManager : MonoBehaviour
{

    public GameObject shopPanel;
    public TextMeshProUGUI totalCoinText;

    private void OnEnable()
    {
        if (GameDataManager.Instance != null && GameDataManager.Instance.playerData != null)
        {
            int totalCoins = GameDataManager.Instance.playerData.totalCoins;
            totalCoinText.text = "���� ����: " + totalCoins.ToString();
        }
        else
        {
            totalCoinText.text = "���� ����: 0";
            Debug.LogWarning("GameDataManager �Ǵ� playerData�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
        }
    }

    public void RefreshCoinText()
    {
        if (GameDataManager.Instance != null && GameDataManager.Instance.playerData != null)
        {
            int totalCoins = GameDataManager.Instance.playerData.totalCoins;
            totalCoinText.text = "���� ����: " + totalCoins.ToString();
        }
        else
        {
            totalCoinText.text = "���� ����: 0";
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        SoundManager.Instance?.PlayShopBGM();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        SoundManager.Instance?.RestorePreviousBGM();
    }
}
