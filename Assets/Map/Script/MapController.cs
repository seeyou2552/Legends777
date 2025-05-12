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

    [SerializeField] private Vector2 obstacleSize = new Vector2(1.3f, 1.3f);
    [SerializeField] private LayerMask obstacleLayer;

    [SerializeField] private float minDistance = 2.0f;
    private List<Vector2> spawnedPositions = new List<Vector2>();

    private int maxAttempts = 30;
    #endregion

    private int stage;
    private DungeonType dungeonType;


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
        GameManager.instance.Stage = 1;
        Init(DungeonType.Lobby);
    }

    public void CreateRandomMap()
    {
        int type = GetRandomType();

        GameManager.instance.Stage++;
        Init((DungeonType)type);
    }

    private int GetRandomType()
    {
        int type;
        while (true)
        {
            type = Random.Range(1, 5);
            if (type != (int)GameManager.instance.DungeonType)
            {
                return type;
            }
        }
    }

    public void Init(DungeonType dungeonType)
    {
        ClearMap();

        GameManager.instance.DungeonType = dungeonType;
        this.dungeonType = dungeonType;


        //stage 정보 받아오기?
        stage = GameManager.instance.Stage;

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
        //quest npc 생성
        SpawnNpcOrBox();
    }

    private void InitMonsterMap()
    {
        //장애물 랜덤으로 생성
        CreateObstacle(stage);
    }

    private void InitBossMap()
    {
        //아무것도 없는 상태
    }

    private void InitItemMap()
    {
        //아이템 생성
        SpawnNpcOrBox();
    }

    private void InitStoreMap()
    {
        //store npc 생성
        SpawnNpcOrBox();
    }

    private void SpawnNpcOrBox()
    {
        if (dungeonType == DungeonType.Lobby)
        {
            QuestManager.Instance.Init();
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

    //장애물 랜덤으로 생성
    private void CreateObstacle(int stage)
    {
        //랜덤 개수
        int obstacleNum = Random.Range(stage - 1, stage + 1);

        //랜덤 위치
        for (int i = 0; i < obstacleNum; i++)
        {
            Vector2 spawnPos = GetRandomNonOverlappingPosition(i);
            GameObject go = GameObject.Instantiate(obstaclePrefab, Root.transform);
            go.transform.position = spawnPos;
        }

        spawnedPositions.Clear();
    }

    private Vector3 GetRandomNonOverlappingPosition(int i)
    {
        int attempts = 0;
        while (attempts < maxAttempts)
        {
            Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));

            //장애물과 겹치는지 확인
            Collider2D hit = Physics2D.OverlapBox(randomPos, obstacleSize, 0f, obstacleLayer);

            //이미 생성된 위치들과 겹치는지 확인
            bool overlapsWithSpawned = false;
            foreach (Vector2 spawnedPos in spawnedPositions)
            {
                if (Vector2.Distance(randomPos, spawnedPos) < minDistance)
                {
                    overlapsWithSpawned = true;
                    break;
                }
            }

            //겹치지 않았다면 해당 위치 반환
            if (hit == null && !overlapsWithSpawned)
            {
                spawnedPositions.Add(randomPos);
                return randomPos;
            }

            //시도 반복
            attempts++;
        }

        //최대 시도 횟수까지 반복했는데도 안 되면 그냥 랜덤 위치 반환 (겹쳐도 어쩔 수 없지?)
        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}
