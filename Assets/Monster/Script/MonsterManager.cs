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


    
    public List<GameObject> enemyPrefabs; // ������ �� ������ ����Ʈ

    public List<MonsterController> activeMonsters = new List<MonsterController>(); // ���� Ȱ��ȭ�� ����

    private bool monsterSpawnComplite;

    //�� ���� �� ����
    [SerializeField] private float timeBetweenSpawns = 0.2f;
    [SerializeField] private float timeBetweenWaves = 1f;

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
        waveMonsterCount = 100;
    }

    public void StartWave(int waveIdx)
    {
        if (waveIdx <= 0)
        {
            GameManager.instance.EndOfWave();
            return;
        }
       
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
        monsterSpawnComplite = false;
        yield return new WaitForSeconds(timeBetweenWaves);

        for (int i = 0; i < waveMonsterCount; i++)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnRandomMonster();
        }

        monsterSpawnComplite = true;
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
            Random.Range(-24f, 24f),
            Random.Range(-11f, 11f)
        );

        // �� ���� �� ����Ʈ�� �߰�
        GameObject spawnedEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity, this.transform);
        MonsterController enemyController = spawnedEnemy.GetComponent<MonsterController>();
        enemyController.Init();

        activeMonsters.Add(enemyController);
    }

    public void RemoveActiveMonster(MonsterController monster)
    {
        activeMonsters.Remove(monster);
        if (monsterSpawnComplite && activeMonsters.Count == 0)
            GameManager.instance.EndOfWave();
    }
}
