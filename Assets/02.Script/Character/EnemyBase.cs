using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Animator animator;
    public float maxHp; //초기설정 Hp
    public float currentHp;
    public float originMoveSpeed; // 원본 이동속도
    public float moveSpeed; //몬스터이동속도
    public static float decreaseMoveSpeed; // 몬스터 이동속도 감소 수치
    [HideInInspector] public GameObject spawnPos; // 몬스터 스폰 위치
    private int curPathIdx = 0; // 현재 이동 할 위치 인덱스
    [Header ("렌더러")] [SerializeField] private SpriteRenderer rend;
    public EnemyType enemyType;
    [HideInInspector] public bool isDead;
    public float phyDef, magDef; // 물방, 마방
    public static float decreasePhyDef, decreaseMagDef; // 물방, 마방 감소 수치
    public int enemyGold; // 몬스터 골드
    public static int increaseEnemyGold; // 몬스터 골드 증가량
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        originMoveSpeed = moveSpeed;
        currentHp = maxHp;
    }
    
    public void TakeDamage(float damage)
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
        PoolManager.instance.ReturnPool(PoolManager.instance.enemyPool.queMap,gameObject,enemyType);
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
        transform.position = Vector2.MoveTowards(transform.position, StageManager.instance.stageTypePathMap[spawnPos].gameObjectList[curPathIdx].transform.position, (moveSpeed - decreaseMoveSpeed) * Time.deltaTime);
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
