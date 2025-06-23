using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItemInstance : MonoBehaviour
{
    public PowerUpItem itemData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.ApplyPowerUp(itemData);
                Destroy(gameObject);
            }
        }
    }
}