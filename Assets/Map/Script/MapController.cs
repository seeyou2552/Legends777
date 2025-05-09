using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DungeonType
{
    Lobby,
    Monster,
    Boss,
    Item,
    Store,
}

public class MapController : MonoBehaviour
{
    #region Prefab
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private GameObject questNpcPrefab;
    [SerializeField] private GameObject storeNpcPrefab;
    [SerializeField] private GameObject itemPrefab;
    #endregion

    #region Obstacle Info
    [SerializeField] private float minX = -8.8f;
    [SerializeField] private float maxX = 8.8f;
    [SerializeField] private float minY = -3.9f;
    [SerializeField] private float maxY = 3.7f;

    [SerializeField] private Vector2 obstacleSize = new Vector2(1.7f, 1.7f);
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float minDistance = 2.0f;
    private List<Vector2> spawnedPositions = new List<Vector2>();

    private int maxAttempts = 30;
    #endregion
    [SerializeField] private int stage;
    [SerializeField] private DungeonType dungeonType;

    private GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@Map_Root");
            if (root == null) 
                root = new GameObject { name  = "@Map_Root" };
            return root;
        }
    }

    private void Start()
    {
        stage = 1;
        Init(DungeonType.Lobby);
    }

    public void CreateRandomMap()
    {
        int type = GetRandomType();
        Debug.Log("Go to" + (DungeonType)type);
        Init((DungeonType)type);
    }

    private int GetRandomType()
    {
        int type;
        while (true)
        {
            type = Random.Range(1, 5);
            if (type != (int)dungeonType)
                return type;
        }
    }

    public void Init(DungeonType dungeonType)
    {
        ClearMap();

        this.dungeonType = dungeonType;
        //stage ���� �޾ƿ���?

        switch (dungeonType)
        {
            case DungeonType.Lobby:
                InitLobbyMap();
                break;
            case DungeonType.Monster:
                InitMonsterMap();
                break;
            case DungeonType.Boss:
                InitBossMap();
                break;
            case DungeonType.Item:
                InitItemMap();
                break;
            case DungeonType.Store:
                InitStoreMap();
                break;
        }
    }

    private void InitLobbyMap()
    {
        //quest npc ����
        SpawnNpcOrBox();
    }

    private void InitMonsterMap()
    {
        //��ֹ� �������� ����
        CreateObstacle(stage);

    }

    private void InitBossMap()
    {
        //�ƹ��͵� ���� ����

    }

    private void InitItemMap()
    {
        //������ ����
        SpawnNpcOrBox();
    }

    private void InitStoreMap()
    {
        //store npc ����
        SpawnNpcOrBox();
    }

    private void SpawnNpcOrBox()
    {
        if (dungeonType == DungeonType.Lobby)
        {
            GameObject go = Instantiate<GameObject>(questNpcPrefab, Root.transform);
            go.transform.position = new Vector2(0, 2);
        }
        else if (dungeonType == DungeonType.Item)
        {
            GameObject go = Instantiate<GameObject>(itemPrefab, Root.transform);
            go.transform.position = new Vector2(0, 2);
        }
        else if (dungeonType == DungeonType.Store)
        {
            GameObject go = Instantiate<GameObject>(storeNpcPrefab, Root.transform);
            go.transform.position = new Vector2(0, 2);
        }
    }

    private void ClearMap()
    {
        Transform mapContent = Root.transform;
        foreach (Transform child in mapContent)
        {
            Destroy(child.gameObject);
        }
    }

    //��ֹ� �������� ����
    private void CreateObstacle(int stage)
    {
        //���� ����
        int obstacleNum = Random.Range(stage - 1, stage + 1);

        //���� ��ġ
        for (int i = 0; i < obstacleNum; i++)
        {
            Vector2 spawnPos = GetRandomNonOverlappingPosition(i);
            GameObject go = GameObject.Instantiate(obstaclePrefab, Root.transform);
            go.transform.position = spawnPos;
        }

    }

    private Vector3 GetRandomNonOverlappingPosition(int i)
    {
        int attempts = 0;
        while (attempts < maxAttempts)
        {
            Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            //��ֹ��� ��ġ���� Ȯ��
            Collider2D hit = Physics2D.OverlapBox(randomPos, obstacleSize, 0f, obstacleLayer);

            //�̹� ������ ��ġ��� ��ġ���� Ȯ��
            bool overlapsWithSpawned = false;
            foreach (Vector2 spawnedPos in spawnedPositions)
            {
                if (Vector2.Distance(randomPos, spawnedPos) < minDistance)
                {
                    overlapsWithSpawned = true;
                    break;
                }
            }

            //��ġ�� �ʾҴٸ� �ش� ��ġ ��ȯ
            if (hit == null && !overlapsWithSpawned)
            {
                spawnedPositions.Add(randomPos);
                return randomPos;
            }

            //�õ� �ݺ�
            attempts++;
        }

        //�ִ� �õ� Ƚ������ �ݺ��ߴµ��� �� �Ǹ� �׳� ���� ��ġ ��ȯ (���ĵ� ��¿ �� ����?)
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}
