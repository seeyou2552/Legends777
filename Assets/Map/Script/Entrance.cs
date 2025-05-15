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

            //ĳ���� ��ġ�� ���� > ������ , ������ > ���� �Ǵ�? �׳� �߾����� �̵��ǵ��� �ϸ� ���� �� ������
            collision.gameObject.transform.position = new Vector2(0, 0);
        }
    }
}
