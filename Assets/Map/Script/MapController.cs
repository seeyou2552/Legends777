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
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private DungeonType dungeonType;
    public DungeonType DungeonType { get { return dungeonType; } }

    public void Init()
    {
        switch(dungeonType)
        {
            case DungeonType.Lobby:
                LobbyInit();
                break;
            case DungeonType.Monster:
                break;
            case DungeonType.Boss:
                break;
            case DungeonType.Item:
                break;
            case DungeonType.Store:
                break;
        }
    }

    //장애물 랜덤으로 생성
    private void CreateObstacle()
    {

    }
}
