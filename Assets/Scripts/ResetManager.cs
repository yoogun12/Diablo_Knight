using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public ShopUiManager shopUiManager;  // 메인 메뉴에 있으면 연결
    public ShopManager shopManager;      // 있으면 연결

    public void ResetGameProgress()
    {
        Debug.Log("메인 메뉴에서 게임 진행 초기화");

        // PlayerPrefs 초기화
        PlayerPrefs.SetInt("HP_LEVEL", 0);
        PlayerPrefs.SetInt("SPEED_LEVEL", 0);
        PlayerPrefs.Save();

        // GameDataManager 영구 저장 코인 초기화
        if (GameDataManager.Instance != null && GameDataManager.Instance.playerData != null)
        {
            GameDataManager.Instance.playerData.totalCoins = 0;
            GameDataManager.Instance.SaveData(GameDataManager.Instance.playerData);
        }

        // UI 갱신
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