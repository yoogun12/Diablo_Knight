using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    [SerializeField] CoinItemSo data;

    public int GetCoin()
    {
        return data.coin;
    }
}
