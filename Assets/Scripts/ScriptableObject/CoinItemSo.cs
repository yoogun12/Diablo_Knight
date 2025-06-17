using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item", fileName = "NewItem")]

public class CoinItemSo : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int value;
}
