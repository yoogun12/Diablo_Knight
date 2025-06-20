using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpItem", menuName = "Game/PowerUpItem")]
public class PowerUpItem : ScriptableObject
{
    public string itemName;
    public Sprite icon;

    public enum PowerUpType { BulletCount, BulletSpeed, MoveSpeed, Damage, FireRate }
    public PowerUpType powerUpType;

    public float powerUpValue;  // �󸶳� �÷����� (��: 0.1f = 10% ����)
}