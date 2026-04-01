using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Scriptable Objects/Enemy")]
public class Enemy : ScriptableObject
{

    [Header("Visuals")]
    public string enemyName;
    public Sprite enemySprite;
    public GameObject enemyPrefab;

    [Header("Stats")]
    public int health;
    public float speed;
    public int damage;

    [Header("Effects")]
    public AudioClip deathSound;

    [Header("Loot")]
    public GameObject ExperiencePoints;
    public GameObject Gold;
}
