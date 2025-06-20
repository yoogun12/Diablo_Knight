using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpItem", menuName = "Game/PowerUpItem")]
public class PowerUpItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public enum PowerUpType { BulletCount, BulletSpeed, MoveSpeed, Damage, FireRate }
    public PowerUpType powerUpType;

    public float powerUpValue;  // 얼마나 올려줄지 (예: 0.1f = 10% 증가)
}