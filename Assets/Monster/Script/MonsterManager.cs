using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    //웨이브
    private Coroutine waveRoutine;
    private bool waveStarted = false;


    [SerializeField]
    private List<GameObject> enemyPrefabs; // 생성할 적 프리팹 리스트

    private List<MonsterController> activeEnemies = new List<MonsterController>(); // 현재 활성화된 적들

    private bool enemySpawnComplite;

    //적 생성 간 간격
    [SerializeField] private float timeBetweenSpawns = 0.2f;

    //한 웨이브에 몇 개의 몬스터 생성할 것인지?
    [SerializeField] private int waveMonsterCount;

    //발사할 투사체 프리팹 배열 (총알 종류별)
    [SerializeField] public GameObject[] projectilePrefabs;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Stage Type이 Monster로 변경되면 몬스터 스폰 (웨이브 시작)
        waveMonsterCount = 5;
        GameManager.instance.OnDungeonTypeMonsterUpdated += StartWave;
    }

    private void Awake() // 추가 내용
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

        // 랜덤한 적 프리팹 선택
        GameObject randomPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // 랜덤한 영역 선택
        Vector2 randomPosition = new Vector2(
            Random.Range(-9, 10),
            Random.Range(-4, 5)
        );

        // 적 생성 및 리스트에 추가
        GameObject spawnedEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity, this.transform);
        MonsterController enemyController = spawnedEnemy.GetComponent<MonsterController>();
        enemyController.Init();

        activeEnemies.Add(enemyController);
    }
}
