using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class PlayerManager : Player
{
    Animator animation;
    SpriteRenderer renderer;

    void Awake()
    {
        animation = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Monster"))
        {
            // 데미지 (Hit)
            OnDamage(other);
        }

    }

    void OnDamage(Collider2D other)
    {
        Debug.Log("hit");
        animation.SetBool("IsHit", true);

        gameObject.layer = LayerMask.NameToLayer("DamagedPlayer");
        renderer.color = new Color(1, 1, 1, 0.4f);
        StartCoroutine(OnInvincibility());
    }

    IEnumerator OnInvincibility()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = LayerMask.NameToLayer("Player");
        animation.SetBool("IsHit", false);
        renderer.color = new Color(1, 1, 1, 1);
    }
}
