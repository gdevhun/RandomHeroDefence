using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DamageType
{
    AD, AP
}

enum AttackType
{
    Melee, Ranged
}

enum GradeType
{
    Normal, Elite, Rare, Legend, Hero 
}
public class CharacterBase : MonoBehaviour
{
    public int AttackDamage;
    private Animator anim;
    private float attackSpeed;
    private bool isOnTarget;
    
    void Awake()
    {
        
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
        
        if(other.gameObject.CompareTag("Enemy")&&!isOnTarget)
        {
            isOnTarget = true;
            //Attack(other.gameObject.GetComponent<Enemy>);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOnTarget = false;
    }
}
