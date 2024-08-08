using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    private Animator animator;
    [Header ("최대 체력")] public float maxHp;
    private float currentHp; // 현재 체력
    public float CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            if(currentHp <= 0) Die();
            UpdateHpUI();
        }
    }
    [HideInInspector] public float originMoveSpeed; // 원본 이속
    [Header ("이동 속도")] public float moveSpeed;
    public static float decreaseMoveSpeed; // 이속 감소 수치
    [HideInInspector] public GameObject spawnPos; // 몬스터 스폰 위치
    private int curPathIdx = 0; // 현재 이동 할 위치 인덱스
    private SpriteRenderer rend; // 렌더러
    [HideInInspector] public EnemyType enemyType; // 몬스터 타입
    [HideInInspector] public bool isDead; // 죽었는지 체크
    [Header ("물방 마방")] public float phyDef, magDef;
    public static float decreasePhyDef, decreaseMagDef; // 물방 마방 감소 수치
    [Header ("몬스터 골드")] public int enemyGold;
    public static int increaseEnemyGold; // 몬스터 골드 증가량
    [Header ("체력 필 오브젝트")] [SerializeField] private GameObject hpFillObj;

    // 초기화
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        originMoveSpeed = moveSpeed;
        CurrentHp = maxHp;
    }
    
    // 피격
    public void TakeDamage(float damage) { CurrentHp -= damage; }

    // 죽음
    private void Die()
    {
        animator.SetTrigger("isDead");
        CurrencyManager.instance.AcquireCurrency(enemyGold, true);
        StageManager.instance.instantEnemyList.gameObjectList.Remove(gameObject);
    }
    
    // 죽는 애니메이션이 끝날 때 호출
    public void InactiveObj() { PoolManager.instance.ReturnPool(PoolManager.instance.enemyPool.queMap,gameObject,enemyType); }

    // 몬스터 이동
    private void Update() { EnemyMove(); }
    private void EnemyMove()
    {
        if(spawnPos == null) return;
        transform.position = Vector2.MoveTowards(transform.position, StageManager.instance.stageTypePathMap[spawnPos].gameObjectList[curPathIdx].transform.position, decreaseMoveSpeed >= moveSpeed ? 0f : (moveSpeed - decreaseMoveSpeed) * Time.deltaTime);
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

    // 체력 UI 갱신
    private void UpdateHpUI()
    {
        Vector3 curScale = hpFillObj.transform.localScale;
        curScale.y = Mathf.Lerp(0, 2.6f, CurrentHp / maxHp);
        hpFillObj.transform.localScale = curScale;
    }
}
