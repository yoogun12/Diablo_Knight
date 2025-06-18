using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int totalCoins = 0;  // ���� ���� ��
    // �ʿ��ϸ� collectedCoins ����Ʈ�� ���� ����
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
        Debug.Log("���� ������ �����: " + json);
    }

    public PlayerData LoadData()
    {
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("���� ������ �ε��: " + json);
            return data;
        }
        else
        {
            Debug.LogWarning("����� ���� �����Ͱ� �����ϴ�.");
            return new PlayerData();
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void PlayerDead()
    {
        SaveData(playerData); // �ֽ� playerData ����
        SceneManager.LoadScene("GameOver");
    }
}
