using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && GameManager.instance.IsStageClear)
        {
            MapController map = GameObject.Find("Map").GetComponent<MapController>();
            map.CreateRandomMap();

            //캐릭터 위치도 왼쪽 > 오른쪽 , 오른쪽 > 왼쪽 또는? 그냥 중앙으로 이동되도록 하면 좋을 것 같은데
            collision.gameObject.transform.position = new Vector2(0, 0);
        }
    }
}
