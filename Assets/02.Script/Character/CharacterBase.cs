using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Pool;

public enum DamageType  //데미지타입
{
    AD, AP
}

public enum AttackType  //공격방식타입
{
    Melee, Ranged
}

public enum HeroGradeType  //히어로등급
{
    Normal, Elite, Rare, Legend, Myth 
}

public enum HeroDefaultSpriteDir //스프라이트기본 방향
{   //스프라이트렌더러 플립 처리를 위한 enum 클래스
    Left, Right
}
[Serializable]
public class HeroInfo  //히어로 정보 클래스
{
    public DamageType damageType;
    public AttackType attackType;
    public HeroGradeType heroGradeType;
    public UnitType unitType;
    public Sprite unitSprite;
    public int attackDamage;
    public float attackSpeed;
    public HeroDefaultSpriteDir heroDefaultSpriteDir;
}
public class CharacterBase : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isOnTarget;
    public HeroInfo heroInfo;
    private static readonly int IsAttacking = Animator.StringToHash("isAttack");
    /*public delegate void IncreaseDamage(int damage);
    public event IncreaseDamage OnIncreaseDamage;

    public delegate void IncreaseAttackSpeed(float speed);
    public event IncreaseAttackSpeed OnIncreaseAttackSpeed;*/
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
    }

    private void Start()
    {
        //인스펙터 초기 설정 공격력, 공격속도로 초기화
        
    }


    void Update()
    {
        
    }

    // private void Attack(Enemy enemy)
    // {
    //     enemy.hp -= AttackDamage;
    // }

    private void OnTriggerStay2D(Collider2D other)
    {
        //공격범위안에들어왔을 때 적이 머물고 있다면
        if(other.gameObject.CompareTag("Enemy")&&!isOnTarget)  //적(태그)이고, 타게팅중이 아니라면
        {
            isOnTarget = true;  //타게팅 활성화
            
            if (other.gameObject.TryGetComponent(out Transform enemyTrans))
            {
                CalculateSpriteRen(enemyTrans); //방향 계산
                //이펙트 생성.
                if (heroInfo.attackType == AttackType.Melee)
                {
                    anim.SetBool(IsAttacking,true); //공격 애니메이션 활성화
                    //transform을 바탕으로 해당위치에 밀리웨폰생성하기
                }
                else   //원거리 처리
                {
                    //PoolManager.instance.GetPool() 9  0.6500563   0.27
                    //Bullet bullet = BulletManager.instance.GetSpecialBullet(gunTransform.position, bulletRotation);
                    //bullet.additiveDamage = additiveDmg;
                }
                
            }
            
            //Attack(other.gameObject.GetComponent<Enemy>);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOnTarget = false;
        anim.SetBool(IsAttacking,false); //공격 애니메이션 비활성화
    }

    public void CalculateDirection(Transform enemyTrans)
    {
        //발사되는 총알의 방향 구현 로직
        Vector2 bulletDirection = (enemyTrans.position - this.transform.position).normalized; //방향
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg; //각도
        Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward); //쿼터니언
    }

    private void CalculateSpriteRen(Transform enemyTrans) //적을 바라보는 스프라이트렌더러 교체함수
    {  
        Vector2 heroDirection = enemyTrans.position - transform.position;
        if (heroDirection.x > 0)
        {   //오른쪽바라보게 렌더러 교체
            if (heroInfo.heroDefaultSpriteDir == HeroDefaultSpriteDir.Left)
            {
                spriteRenderer.flipX = true;
            }
            else if (heroInfo.heroDefaultSpriteDir == HeroDefaultSpriteDir.Right)
            {
                spriteRenderer.flipX = false;
            }
        }
        else //왼쪽 바라보게 렌더러 교체
        {
            if (heroInfo.heroDefaultSpriteDir == HeroDefaultSpriteDir.Right)
            {
                spriteRenderer.flipX = true;
            }
            else if (heroInfo.heroDefaultSpriteDir == HeroDefaultSpriteDir.Left)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
}
