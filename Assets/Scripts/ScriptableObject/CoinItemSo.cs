using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item", fileName = "NewItem")]

public class CoinItemSo : ScriptableObject
{
    [Header("Coin Value")]
    public int coin = 100;
}
