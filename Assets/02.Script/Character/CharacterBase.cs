using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

[Serializable]
public class HeroInfo  //히어로 정보 클래스
{
    public DamageType damageType;
    public AttackType attackType;
    public HeroGradeType heroGradeType;

}
public class CharacterBase : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public int attackDamage;
    private Animator anim;
    private float attackSpeed;
    private bool isOnTarget;
    public HeroInfo heroInfo;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
    }

    private void Start()
    {
        
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
            //anim.SetBool("Attack"); //공격 애니메이션 활성화
            if (other.gameObject.TryGetComponent(out Transform transform))
            {
                //이펙트 생성.
            }
            //Attack(other.gameObject.GetComponent<Enemy>);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOnTarget = false;
    }
}
