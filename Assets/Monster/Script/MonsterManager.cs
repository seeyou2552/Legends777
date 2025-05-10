using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    //���̺�
    private Coroutine waveRoutine;
    private bool waveStarted = false;


    [SerializeField]
    private List<GameObject> enemyPrefabs; // ������ �� ������ ����Ʈ

    private List<MonsterController> activeEnemies = new List<MonsterController>(); // ���� Ȱ��ȭ�� ����

    private bool enemySpawnComplite;

    //�� ���� �� ����
    [SerializeField] private float timeBetweenSpawns = 0.2f;

    //�� ���̺꿡 �� ���� ���� ������ ������?
    [SerializeField] private int waveMonsterCount;

    //�߻��� ����ü ������ �迭 (�Ѿ� ������)
    [SerializeField] public GameObject[] projectilePrefabs;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Stage Type�� Monster�� ����Ǹ� ���� ���� (���̺� ����)
        waveMonsterCount = 5;
        GameManager.instance.OnDungeonTypeMonsterUpdated += StartWave;
    }

    private void Awake() // �߰� ����
    {
        Instance = this;
    }

    public void StartWave()
    {
        if (waveRoutine != null)
            StopCoroutine(waveRoutine);
        waveRoutine = StartCoroutine(SpawnWave(waveMonsterCount));
    }

    public void StopWave()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnWave(int waveMonsterCount)
    {
        enemySpawnComplite = false;

        for (int i = 0; i < waveMonsterCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomMonster();
        }

        enemySpawnComplite = true;
    }

    public void SpawnRandomMonster()
    {
        if (enemyPrefabs.Count == 0)
        {
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
        MonsterController enemyController = spawnedEnemy.GetComponent<MonsterController>();
        enemyController.Init();

        activeEnemies.Add(enemyController);
    }
}
