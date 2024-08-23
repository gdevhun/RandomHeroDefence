using TMPro;
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
    [Header ("원래 이동 속도")] public float originMoveSpeed; // 원본 이속
    [Header ("이동 속도")] public float moveSpeed;
    private static float decreaseMoveSpeed; // 이속 감소 수치
    public static float DecreaseMoveSpeed
    {
        get { return decreaseMoveSpeed; }
        set
        {
            decreaseMoveSpeed = value;
            UpdateBuffUI(value * 100, UiUnit.instance.slowText);
        }
    }
    [HideInInspector] public GameObject spawnPos; // 몬스터 스폰 위치
    [HideInInspector] public int curPathIdx = 0; // 현재 이동 할 위치 인덱스
    private SpriteRenderer rend; // 렌더러
    [HideInInspector] public EnemyType enemyType; // 몬스터 타입
    [HideInInspector] public bool isDead; // 죽었는지 체크
    [Header ("물방 마방")] public float phyDef, magDef;
    private static float decreasePhyDef; // 물방 감소 수치
    public static float DecreasePhyDef
    {
        get { return decreasePhyDef; }
        set
        {
            decreasePhyDef = value;
            UpdateBuffUI(value, UiUnit.instance.phyDecText);
        }
    }
    private static float decreaseMagDef; // 마방 감소 수치
    public static float DecreaseMagDef
    {
        get { return decreaseMagDef; }
        set
        {
            decreaseMagDef = value;
            UpdateBuffUI(value, UiUnit.instance.magDecText);
        }
    }
    [HideInInspector] public int enemyCurrency;
    [Header ("체력 필 오브젝트")] [SerializeField] private GameObject hpFillObj;
    private GameObject instantStunEffect; // 생성된 스턴 이펙트

    private float curStunTime; // 스턴 시간 계산용
    public float SetStunTime
    {
        get { return curStunTime; }
        set
        {
            // 현재 남은 스턴 시간보다 더 짧은 스턴이 들어오면 리턴
            //if(value <= curStunTime) return;

            // 스턴 시작
            curStunTime = value;
            moveSpeed = 0;
            if(instantStunEffect != null) PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, instantStunEffect, AbilityEffectType.스턴);
            instantStunEffect = PoolManager.instance.GetPool(PoolManager.instance.abilityEffectPool.queMap, AbilityEffectType.스턴);
            instantStunEffect.transform.position = transform.position;
        }
    }

    public static int floatingDmgCnt;

    // 초기화
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }
    private void OnEnable() { curPathIdx = 0; }
    
    // 피격 계산
    // => 물방, 마방 적용
    public void TakeDamage(float damage, DamageType dmgType)
    {
        float applyDmg = damage;

        // 물리
        if(dmgType == DamageType.물리)
        {
            float applyPhyDef = phyDef - DecreasePhyDef;
            applyDmg = applyPhyDef > 0 ? applyDmg * (1 - phyDef / (phyDef + 100)) : damage;
            CurrentHp -= applyDmg;
            if(applyDmg > 0) FloatingDmg(applyDmg);
            return;
        }

        // 마법
        float applyMagDef = magDef - decreaseMagDef;
        applyDmg = applyMagDef > 0 ? applyDmg * (1 - magDef / (magDef + 100)) : damage;
        CurrentHp -= applyDmg;
        if(applyDmg > 0) FloatingDmg(applyDmg);
    }
    private void FloatingDmg(float dmg)
    {
        // 플로팅 데미지 개수
        if(floatingDmgCnt >= 50) return;

        FloatingText instantFloatingText = PoolManager.instance.GetPool(PoolManager.instance.floatingTextPool.queMap, FloatingTextType.데미지플로팅).GetComponent<FloatingText>();
        floatingDmgCnt++;

        // 1000미만 => 그냥
        // 1000000미만 => 1000으로 나누고 K
        // 그 외 => 1000000으로 나누고 M
        if(dmg < 1000) instantFloatingText.text.text = $"{dmg:0.0}";
        else if(dmg < 1000000) instantFloatingText.text.text = $"{dmg / 1000f:0.0}K";
        else instantFloatingText.text.text = $"{dmg / 1000000f:0.0}M";
        
        Vector3 worldToScreen = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 0.5f);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)instantFloatingText.canvasTransform, worldToScreen, null, out Vector2 localPoint);
        instantFloatingText.GetComponent<RectTransform>().localPosition = localPoint;
    }

    // 죽음
    private void Die()
    {
        if(isDead) return;

        animator.SetTrigger("isDead");
        CurrencyManager.instance.AcquireCurrency(enemyCurrency, enemyType.ToString()[0] == 'N');
        StageManager.instance.instantEnemyList.gameObjectList.Remove(gameObject);
        StageManager.instance.EnemyCnt--;
        isDead = true;
    }
    
    // 죽는 애니메이션이 끝날 때 호출
    public void InactiveObj()
    {
        if(instantStunEffect != null) PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, instantStunEffect, AbilityEffectType.스턴);
        PoolManager.instance.ReturnPool(PoolManager.instance.enemyPool.queMap,gameObject,enemyType);
    }

    // 몬스터 이동
    private void Update() { EnemyMove(); GetStun(); }
    private void EnemyMove()
    {
        if(spawnPos == null) return;
        transform.position = Vector2.MoveTowards(transform.position, StageManager.instance.stageTypePathMap[spawnPos].gameObjectList[curPathIdx].transform.position, DecreaseMoveSpeed >= moveSpeed ? 0f : (moveSpeed - DecreaseMoveSpeed) * Time.deltaTime);
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
    private void OnTriggerStay2D(Collider2D other) // 스폰되면 몬스터 스턴되는 문제
    {
        if (spawnPos != null && other.gameObject.name.Equals(StageManager.instance.stageTypePathMap[spawnPos].gameObjectList[0].name))
        {
            moveSpeed = originMoveSpeed;
        }
    }

    // 스턴
    private void GetStun()
    {
        if(curStunTime > 0)
        {
            curStunTime -= Time.deltaTime;

            if(curStunTime <= 0)
            {
                curStunTime = 0;
                moveSpeed = originMoveSpeed;
                if(instantStunEffect != null) PoolManager.instance.ReturnPool(PoolManager.instance.abilityEffectPool.queMap, instantStunEffect, AbilityEffectType.스턴);
            }
        }
    }

    // 체력 UI 갱신
    private void UpdateHpUI()
    {
        Vector3 curScale = hpFillObj.transform.localScale;
        curScale.y = Mathf.Lerp(0, 2.6f, CurrentHp / maxHp);
        hpFillObj.transform.localScale = curScale;
    }

    // 버프 UI 갱신
    private static void UpdateBuffUI(float val, TextMeshProUGUI txt) { txt.text = ((int)val).ToString(); }
}
