using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Pool;

public enum DamageType  //데미지타입
{
    물리, 마법
}

public enum AttackType  //공격방식타입
{
    근거리, 원거리
}

public enum HeroGradeType  //히어로등급
{
    일반, 고급, 희귀, 전설, 신화 
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
    [SerializeField] private WeaponEffect weaponEffect;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    [HideInInspector] public bool isOnTarget;
    public HeroInfo heroInfo;
    private static readonly int IsAttacking = Animator.StringToHash("isAttack");
    public Transform gunPointTrans;
    private float prevAtkSpeed = 0;
    public float limitAtkSpeed;
    private Transform enemyTrans;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (heroInfo.attackType == AttackType.근거리) gunPointTrans = null;
    }

    private void Start()
    {
        //인스펙터 초기 설정 공격력, 공격속도로 초기화
        limitAtkSpeed = heroInfo.attackSpeed;
        prevAtkSpeed = limitAtkSpeed;
    }


    void Update()
    {
        if (prevAtkSpeed < limitAtkSpeed)
        {
            prevAtkSpeed += Time.deltaTime;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        //CalculateSpriteRen(enemyTrans); //방향 계산

        
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyTrans = other.gameObject.transform;
        }
        if(enemyTrans != null) CalculateSpriteRen(enemyTrans);
        
        //공격범위안에들어왔을 때 적이 머물고 있다면
        if(other.gameObject.CompareTag("Enemy") && prevAtkSpeed >= limitAtkSpeed)  //적(태그)이고, 타게팅중이 아니라면
        {
            isOnTarget = true;  //타게팅 활성화
            prevAtkSpeed = 0f;
            
            //이펙트 생성.
            if (heroInfo.attackType == AttackType.근거리)
            {
                anim.SetBool(IsAttacking,true); //공격 애니메이션 활성화
                GameObject go = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, weaponEffect);
                go.transform.position = enemyTrans.position;
                go.GetComponent<MeleeWeapon>().weaponEffect = weaponEffect;
                go.GetComponent<MeleeWeapon>().attackDamage = heroInfo.attackDamage;
                //transform을 바탕으로 해당위치에 밀리웨폰생성하기
            }
            else   //원거리 처리
            {
                GameObject go = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, weaponEffect);
                go.GetComponent<RangeWeapon>().weaponEffect = weaponEffect;
                go.GetComponent<RangeWeapon>().attackDamage = heroInfo.attackDamage;
                SetLastBulletPos(go,enemyTrans);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOnTarget = false;
        if (heroInfo.attackType == AttackType.근거리)
        {
            anim.SetBool(IsAttacking, false); //공격 애니메이션 비활성화
        }
    }

    private Quaternion CalculateBulletRotation(Transform enemyTrans)
    {
        //발사되는 총알의 방향 구현 로직
        Vector2 bulletDirection = (enemyTrans.position - this.transform.position); //방향계산
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg; //방향을 기반으로 각도 계산
        Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward); //쿼터니언 계산
        return bulletRotation;
    }
    private void SetLastBulletPos(GameObject bullet,Transform enemyTrans) //최종 총알 발사 입구 설정
    {
        bullet.transform.SetPositionAndRotation(gunPointTrans.position,CalculateBulletRotation(enemyTrans));
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
