using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class CoinItem : MonoBehaviour
{
    public CoinItemSo itemData; // ����� ������ ������ (ScriptableObject)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //CoinInventory coinInventory = other.GetComponent<CoinInventory>();
           // if (coinInventory != null && itemData != null)
           // {
           //     coinInventory.AddCoins(itemData.value);
           // }

            Destroy(gameObject); // ���� ������Ʈ ����
        }
    }
}
