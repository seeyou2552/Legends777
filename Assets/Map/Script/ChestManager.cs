using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public GameObject hp_Potion;
    public GameObject power_Potion;
    public GameObject attackSpeed_Potion;
    List<GameObject> potionList = new List<GameObject>();

    public bool interChest = false;

    void Awake()
    {
        potionList.AddRange(new[] { hp_Potion, power_Potion, attackSpeed_Potion });
    }

    void Update()
    {
        if (interChest && Input.GetKeyDown(KeyCode.Z))
        {
            RandomPotion();
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interChest = true;
        }
    }

    void OiggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interChest = false;
        }
    }

    void RandomPotion()
    {
        int index = Random.Range(0, potionList.Count);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1, 0f);
        Instantiate(potionList[index], spawnPos, Quaternion.identity);
    }
}
