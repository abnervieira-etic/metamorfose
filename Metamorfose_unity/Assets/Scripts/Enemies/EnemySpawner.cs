using UnityEngine;
using System.Collections;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints; 
    [SerializeField] private GameObject[] enemyPrefabs; 
    [SerializeField] private GameObject bossPrefab;

    void Start()
    {
        
    }
    public void SpawnRound(int enemyCount, float spawnInterval, bool spawnBoss)
    {
        StartCoroutine(SpawnRoutine(enemyCount, spawnInterval, spawnBoss));
    }

    IEnumerator SpawnRoutine(int enemyCount, float spawnInterval, bool spawnBoss)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnOne(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]);
            yield return new WaitForSeconds(spawnInterval);
        }

        if (spawnBoss)
        {
            SpawnOne(bossPrefab);
        }
    }

    void SpawnOne(GameObject prefab)
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(prefab, point.position, point.rotation);
        RoundManager.Instance.RegisterEnemy(enemy);
    }
}
