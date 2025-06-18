using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int totalCoins = 0;  // 누적 코인 수
    // 필요하면 collectedCoins 리스트도 유지 가능
}

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public PlayerData playerData;

    private string filePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            filePath = Application.persistentDataPath + "/Player_data.json";
            playerData = LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("게임 데이터 저장됨: " + json);
    }

    public PlayerData LoadData()
    {
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("게임 데이터 로드됨: " + json);
            return data;
        }
        else
        {
            Debug.LogWarning("저장된 게임 데이터가 없습니다.");
            return new PlayerData();
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void PlayerDead()
    {
        SaveData(playerData); // 최신 playerData 저장
        SceneManager.LoadScene("GameOver");
    }
}
