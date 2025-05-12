using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_FireBall : MonoBehaviour
{
    //기본 패턴
    // 보스의 기본공격은 파이어볼을 하나씩 2번날리고 3번째에 사방으로 파이어볼 발사 루틴 파이어 볼은 한번 벽에 튕기고 2번째 벽에 닿으면 파괴
    // 스킬 시전이 끝나고 2초 동안 움직임x
    // 보스 몹과 충돌시 데미지 // 미구현
    public float speed = 15f;
    private Vector2 direction;

    // 방향 설정 함수
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    // 매 프레임 이동 처리
    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    // 화면 밖으로 나가면 풀로 반환
    private void OnBecameInvisible()
    {
        ReturnToPool();
    }

    // 충돌 시 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.hp -= 10;
            Debug.Log("Player HP: " + Player.Instance.hp);
            if(Player.Instance.hp <= 0)
            {
                Destroy(BossManager.instance.PlayerTarget);
            }
            ReturnToPool();
        }
        // 몬스터나 다른 파이어볼과 충돌하지 않았을 때에만 반환
        else if (!collision.gameObject.CompareTag("Monster") && !collision.gameObject.CompareTag("FireBall"))
        {
            ReturnToPool();
        }
    }

    // 오브젝트 풀로 반환하는 함수
    private void ReturnToPool()
    {
        BossObjectPoolManager.Instance.ReturnToPool("Fireball", gameObject);
    }


    // 스킬 및 관문별 사용 가능 스킬
    // 스킬 시전은 5초마다 한번씩 진행
    // 플레이어에게 빠르게 돌진 1 ~ 완
    // 잡몹들 소환 3 완

    // 파이어볼 연속으로 5초간 연사 플레이어를 조준하면서 4 ~ 완
    // 레이저(빨강) 십자가 발사 후 1초 뒤 x자로 발사 이건 벽을 관통 6 완

    // 구체를 자신의 몸주위로 빠르게 돌리다가 한번에 발사 7 ~
    // 카메라 반전 효과 10초간 유지 9 완

    // 화면을 빨강색으로 바꿔서 공격 및 지형이 보이지 않게 10초간 유지 10 ~ 완
    // 구현 가능하면 피 절반 일떄 분열 피가 50%면 둘다 피 50%를 가진상태로 분열 
}
