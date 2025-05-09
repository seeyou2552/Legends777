using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class MonsterManager : MonoBehaviour
{
    private Coroutine waveRoutine;
    private bool waveStarted = false;
    [SerializeField]
    private List<GameObject> enemyPrefabs; // 생성할 적 프리팹 리스트

    private List<Monster> activeEnemies = new List<Monster>(); // 현재 활성화된 적들

    private bool enemySpawnComplite;

    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;


    public void StartWave(int waveCount)
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);
        waveRoutine = StartCoroutine(SpawnWave(waveCount));
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveCount)
    {
        enemySpawnComplite = false;
        yield return new WaitForSeconds(timeBetweenWaves);
        for (int i = 0; i < waveCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomMonster();
        }

        enemySpawnComplite = true;
    }

    private void SpawnRandomMonster()
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("!!!");
            return;
        }

        // 랜덤한 적 프리팹 선택
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // 랜덤한 영역 선택
        Vector2 randomPosition = new Vector2(
            Random.Range(-9, 10),
            Random.Range(-4, 5)
        );

        // 적 생성 및 리스트에 추가
        GameObject spawnedEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity, this.transform);
        Monster enemyController = spawnedEnemy.GetComponent<Monster>();

        activeEnemies.Add(enemyController);
    }
}
