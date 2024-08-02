using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private int maxHp; //초기설정 Hp
    private int currentHp;
    [SerializeField] private int moveSpeed; //몬스터이동속도
    [HideInInspector] public GameObject spawnPos; // 몬스터 스폰 위치
    private int curPathIdx = 0; // 현재 이동 할 위치 인덱스
    [Header ("렌더러")] [SerializeField] private SpriteRenderer rend;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHp = maxHp;
    }
    
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Enemy took {damage} damage, remaining HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        //풀에 다시 집어넣기
    }

    public void InactiveObj()
    {
        gameObject.SetActive(false); //에니메이션 데드이벤트 끝쪽에서 호출
    }
    void Update()
    {
        // 몬스터 이동
        EnemyMove();
    }

    // 몬스터 이동
    private void EnemyMove()
    {
        if(spawnPos == null) return;
        transform.position = Vector2.MoveTowards(transform.position, StageManager.instance.stageTypePathMap[spawnPos].gameObjectList[curPathIdx].transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (spawnPos != null && other.gameObject.name.Equals(StageManager.instance.stageTypePathMap[spawnPos].gameObjectList[curPathIdx].name))
        {
            // 이동 위치 갱신
            curPathIdx++;
            if (curPathIdx >= StageManager.instance.stageTypePathMap[spawnPos].gameObjectList.Count) curPathIdx = 0;
            
            // 플립
            rend.flipX = StageManager.instance.stageTypePathMap[spawnPos].gameObjectList[curPathIdx].transform.position.x < transform.position.x ? true : false;
        }
    }
}
