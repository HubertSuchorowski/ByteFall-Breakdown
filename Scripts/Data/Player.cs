using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Scriptable Objects/Player")]
public class Player : ScriptableObject
{
    [Header("Visuals")]
    public string playerName;
    public Sprite playerSprite;
    public GameObject playerPrefab;

    [Header("Stats")]
    public int maxHealth;
    public int regenRate;
    public float lifeStealPercentage;

    public int critMultiplayer;
    public int critChance;
    
    public float speed;

    public int ExperiencePoints;
}