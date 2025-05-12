using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObjectPoolManager : MonoBehaviour
{
    public static BossObjectPoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;            // 고유 태그 이름 (예: "Fireball", "RedGround")
        public GameObject prefab;     // 생성할 프리팹
        public int size;              // 초기 풀 크기
    }

    public List<Pool> pools;  // 인스펙터에서 설정할 풀 리스트
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 각 풀마다 오브젝트 미리 생성해서 큐에 저장
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, this.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // 오브젝트 풀에서 꺼내기
    public GameObject GetFromPool(string tag, Vector2 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"{tag} 태그가 없습니다.");
            return null;
        }

        GameObject obj;
        if (poolDictionary[tag].Count > 0)
        {
            obj = poolDictionary[tag].Dequeue();
            if (obj == null) // 이미 파괴된 오브젝트를 반환하려는 경우
            {
                obj = Instantiate(GetPrefab(tag), parent);
            }
        }
        else
        {
            obj = Instantiate(GetPrefab(tag), parent);
        }

        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    // 오브젝트 풀로 반환
    public void ReturnToPool(string tag, GameObject obj)
    {
        if (obj == null || !obj.activeSelf) // 이미 비활성화되었거나 파괴된 오브젝트는 반환하지 않음
        {
            return;
        }

        obj.SetActive(false); // 오브젝트 비활성화
        poolDictionary[tag].Enqueue(obj); // 풀에 다시 넣기
    }

    // 태그로 프리팹 찾기
    private GameObject GetPrefab(string tag)
    {
        return pools.Find(p => p.tag == tag)?.prefab;
    }
}
