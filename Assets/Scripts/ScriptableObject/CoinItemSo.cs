using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item", fileName = "NewItem")]

public class CoinItemSo : ScriptableObject
{
    public int coinID; // 코인 고유 ID
    [Header("Coin Value")]
    public int coin = 100;
}
