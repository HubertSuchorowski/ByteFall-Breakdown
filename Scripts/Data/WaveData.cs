using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class EnemySpawnConfig
{
    public GameObject enemyPrefab;
    [Range(0, 100)] 
    public float spawnChance = 10f; 
}

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [Header("Ustawienia Fali")]
    public float spawnRate = 1.0f; 

    [Header("Lista Wrogów")]
    public List<EnemySpawnConfig> enemiesInWave; 
}