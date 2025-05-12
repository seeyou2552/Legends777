using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObjectPoolManager : MonoBehaviour
{
    public static BossObjectPoolManager Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;            // ���� �±� �̸� (��: "Fireball", "RedGround")
        public GameObject prefab;     // ������ ������
        public int size;              // �ʱ� Ǯ ũ��
    }

    public List<Pool> pools;  // �ν����Ϳ��� ������ Ǯ ����Ʈ
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // �� Ǯ���� ������Ʈ �̸� �����ؼ� ť�� ����
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

    // ������Ʈ Ǯ���� ������
    public GameObject GetFromPool(string tag, Vector2 position, Quaternion rotation, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"{tag} �±װ� �����ϴ�.");
            return null;
        }

        GameObject obj;
        if (poolDictionary[tag].Count > 0)
        {
            obj = poolDictionary[tag].Dequeue();
            if (obj == null) // �̹� �ı��� ������Ʈ�� ��ȯ�Ϸ��� ���
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

    // ������Ʈ Ǯ�� ��ȯ
    public void ReturnToPool(string tag, GameObject obj)
    {
        if (obj == null || !obj.activeSelf) // �̹� ��Ȱ��ȭ�Ǿ��ų� �ı��� ������Ʈ�� ��ȯ���� ����
        {
            return;
        }

        obj.SetActive(false); // ������Ʈ ��Ȱ��ȭ
        poolDictionary[tag].Enqueue(obj); // Ǯ�� �ٽ� �ֱ�
    }

    // �±׷� ������ ã��
    private GameObject GetPrefab(string tag)
    {
        return pools.Find(p => p.tag == tag)?.prefab;
    }
}
