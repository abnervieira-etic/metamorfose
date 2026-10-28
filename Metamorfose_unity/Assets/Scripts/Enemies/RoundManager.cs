using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance { get; private set; }

    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private int baseEnemyCount = 2;
    [SerializeField] private int enemyIncreasePerRound = 1;
    [SerializeField] private float baseSpawnInterval = 2f;
    [SerializeField] private float spawnIntervalDecreasePerRound = 0.15f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private int bossEveryNRounds = 5;
    [SerializeField] private float timeBetweenRounds = 5f;
    [SerializeField] TMP_Text roundNumberTxt;
    public int CurrentRound { get; private set; } = 0;
    private List<GameObject> aliveEnemies = new List<GameObject>();

    void Start()
    {
        Instance = this;
        roundNumberTxt.enabled = false;
        StartNextRound();
    }
    void Update()
    { 
        roundNumberTxt.text = CurrentRound.ToString();
    }
    public void StartNextRound()
    {   
        CurrentRound++;

        roundNumberTxt.enabled = true;
        
        int enemyCount = baseEnemyCount + (CurrentRound - 2) * enemyIncreasePerRound;
        float spawnInterval = Mathf.Max(minSpawnInterval, baseSpawnInterval - (CurrentRound - 1) * spawnIntervalDecreasePerRound);
        bool spawnBoss = CurrentRound % bossEveryNRounds == 0;

        spawner.SpawnRound(enemyCount, spawnInterval, spawnBoss);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        aliveEnemies.Add(enemy);
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);

        if (aliveEnemies.Count == 0)
        {
            Invoke(nameof(StartNextRound), timeBetweenRounds);
        }
    }
}