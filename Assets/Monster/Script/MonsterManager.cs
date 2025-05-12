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
    private List<GameObject> enemyPrefabs; // ������ �� ������ ����Ʈ

    private List<Monster> activeEnemies = new List<Monster>(); // ���� Ȱ��ȭ�� ����

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

        // ������ �� ������ ����
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // ������ ���� ����
        Vector2 randomPosition = new Vector2(
            Random.Range(-9, 10),
            Random.Range(-4, 5)
        );

        // �� ���� �� ����Ʈ�� �߰�
        GameObject spawnedEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity, this.transform);
        Monster enemyController = spawnedEnemy.GetComponent<Monster>();

        activeEnemies.Add(enemyController);
    }
}
