using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class KeyTileInfo
{
    public KeyCode key;             // 예: KeyCode.W
    public Vector3Int cellPosition; // 타일맵 셀 좌표 (ex: new Vector3Int(0, 1, 0))
    public TileBase normalTile;     // 평상시 타일 에셋
    public TileBase pressedTile;    // 눌림 상태 타일 에셋
}

public class TutorialController : MonoBehaviour
{
    [Header("— Tilemap & Keys 설정 —")]
    [SerializeField] private Tilemap tutorialTilemap;
    [SerializeField] private List<KeyTileInfo> keyTiles = new List<KeyTileInfo>();

    bool isActive = false;

    void Update()
    {
        // 1) 로비일 때만 토글
        if (GameManager.instance.DungeonType == DungeonType.Lobby)
        {
            if (!isActive)
            {
                ShowTutorialTiles();
                isActive = true;
            }
            UpdateTilesByInput();
        }
        else if (isActive)
        {
            ClearTutorialTiles();
            isActive = false;
        }
    }

    private void ShowTutorialTiles()
    {
        // 로비 입장 시, 모든 키에 대해 '평상시' 타일 세팅
        foreach (var info in keyTiles)
            tutorialTilemap.SetTile(info.cellPosition, info.normalTile);
    }

    private void ClearTutorialTiles()
    {
        // 로비 벗어날 때 전부 삭제
        tutorialTilemap.ClearAllTiles();
    }

    private void UpdateTilesByInput()
    {
        // 매 프레임, 각 키가 눌려 있는지 검사해서 타일 교체
        foreach (var info in keyTiles)
        {
            bool pressed = Input.GetKey(info.key);
            tutorialTilemap.SetTile(
                info.cellPosition,
                pressed ? info.pressedTile : info.normalTile
            );
        }
    }
}
