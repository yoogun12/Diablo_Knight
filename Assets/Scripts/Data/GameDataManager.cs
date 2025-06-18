using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerData
{
    public List<int> collectedCoins = new List<int>();
}   

    
public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;
    public PlayerData playerData;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData(PlayerData playerData)
    {
        string filePath = Application.persistentDataPath + "/Player_data_json";
        string json = JsonUtility.ToJson(playerData, true);
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("���� ������ �����: " + json);
    }

    public PlayerData LoadData()
    {
        string filePath = Application.persistentDataPath + "/Player_data_json";
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("���� ������ �ε��: " + json);
            return playerData;
        }
        else
        {
            Debug.LogWarning("����� ���� �����Ͱ� �����ϴ�.");
            return new PlayerData();
        }

    }

    public void GameStart()
    {
        playerData = LoadData();
        if(playerData == null)
        {
            playerData = new PlayerData();
            SceneManager.LoadScene("GamePlay");
        }
    }   

    public void PlayerDead()
    {
        PlayerData playerData = LoadData();
        if (playerData != null)
        {
            SaveData(playerData);
        }
        SceneManager.LoadScene("GameOver");
    }
    

}
