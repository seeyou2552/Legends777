using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBossItem : MonoBehaviour
{
    [SerializeField] private Sprite[] itemSprite;
    Player player;
    
    private void Start()
    {
        player = Player.Instance;

        int Srandom = Random.Range(0, 4);

        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.sprite = itemSprite[Srandom];

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            int random = Random.Range(0, 2);
            if (random == 0)
            {
                MakeRandomEffect();
            }
            else
            {
                MakeRandomBedEffect();
            }
            BossObjectPoolManager.Instance.ReturnToPool("item", this.gameObject);
        }
    }
    private void MakeRandomEffect()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                player.attackSpeed *= 2;
                Debug.Log("+asp");
                break;
            case 1:
                player.speed *= 2;
                Debug.Log("+sp");
                break;
            case 2:
                player.hp *= 2;
                Debug.Log("+hp");
                break;
            case 3:
                player.power *= 2;
                Debug.Log("+po");
                break;
            default:
                Debug.LogError("noEffect");
                break;
        }
    }

    private void MakeRandomBedEffect()
    {
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                player.attackSpeed /= 2;
                Debug.Log("-asp");
                break;
            case 1:
                player.speed /= 2;
                Debug.Log("-sp");
                break;
            case 2:
                player.hp /= 2;
                Debug.Log("-hp");
                break;
            case 3:
                player.power /= 2;
                Debug.Log("-po");
                break;
            default:
                Debug.LogError("noBedEffect");
                break;
        }
    }
}
