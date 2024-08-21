using System;
using UnityEngine;

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
    [Header ("데미지 타입")] public DamageType damageType;
    [Header ("공격방식 타입")] public AttackType attackType;
    [Header ("유닛 등급")]public HeroGradeType heroGradeType;
    [Header ("유닛 타입")]public UnitType unitType;
    [Header ("유닛 스프라이트")] public Sprite unitSprite;
    [Header ("공격력")] public float attackDamage;
    [Header ("공격 속도")] public float attackSpeed;
    [Header ("초기 렌더러 방향")] public HeroDefaultSpriteDir heroDefaultSpriteDir;
}
public class CharacterBase : MonoBehaviour
{
    [Header ("평타 이펙트 타입")] public WeaponEffect weaponEffect;
    private SpriteRenderer spriteRenderer; // 렌더러
    private Animator anim; // 애니메이션
    [HideInInspector] public bool isOnTarget; // 타겟이 있는지 체크
    public HeroInfo heroInfo; // 유닛 정보
    private static readonly int IsAttacking = Animator.StringToHash("isAttack");
    [HideInInspector] public Transform gunPointTrans; // 원거리 발사 위치
    private float prevAtkSpeed = 0; // 공속 계산용
    [HideInInspector] public float limitAtkSpeed; // 공속
    [HideInInspector] public Transform enemyTrans; // 타겟 몬스터 트랜스폼
    [Header ("평타 사운드 타입")] [SerializeField] private SoundType atkSoundType;
    private CircleCollider2D coll; // 콜라이더
    [Header ("사정거리 표시")] public GameObject indicateAttackRange;
    public bool FlipX
    {
        get { return spriteRenderer.flipX; }
        set
        {
            // 원거리 발사 위치 재설정
            if(heroInfo.attackType == AttackType.원거리 && spriteRenderer.flipX != value)
            {
                Vector3 newGunPos = new Vector3(-gunPointTrans.localPosition.x, gunPointTrans.localPosition.y, gunPointTrans.localPosition.z);
                gunPointTrans.localPosition = newGunPos;
            }
            spriteRenderer.flipX = value;
        }
    }
    
    // 초기화
    void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (heroInfo.attackType == AttackType.근거리) gunPointTrans = null;
        indicateAttackRange.transform.localScale = new Vector3(coll.radius * 2, coll.radius * 2, coll.radius * 2);
    }
    private void Start()
    {
        limitAtkSpeed = heroInfo.attackSpeed;
        prevAtkSpeed = limitAtkSpeed;
    }

    //머니건 활성화되면 계산더해지는 함수
    private float ApplyLastAttackDamage(float attackDamage)
    {
        if (MissionManager.instance.isMoneyGunActive)
        {
            attackDamage *= 1 + CurrencyManager.instance.Gold * 0.00002f;
        }
        return attackDamage;
    }
    // 적용 할 공격력 계산
    // 물공 업그레이드, 마공 업그레이드, 등급 업그레이드 적용
    public float GetApplyAttackDamage(float basicAttackDamage)
    {
        float applyAttack = heroInfo.damageType == DamageType.물리
            ? (basicAttackDamage +
               basicAttackDamage * UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] / 100
               + basicAttackDamage * UpgradeUnit.instance.gradeUpgradeMap[heroInfo.heroGradeType] / 100)
            : (basicAttackDamage +
               basicAttackDamage * UpgradeUnit.instance.damageUpgradeMap[DamageType.마법] / 100
               + basicAttackDamage * UpgradeUnit.instance.gradeUpgradeMap[heroInfo.heroGradeType] / 100);
            
        return ApplyLastAttackDamage(applyAttack);
    }

    // 공속 계산
    void Update() { if (prevAtkSpeed < limitAtkSpeed) prevAtkSpeed += Time.deltaTime; }

    // 타겟 공격
    private void OnTriggerStay2D(Collider2D other)
    {
        // 드래그 체크
        if(SelectUnit.instance.isDrag && transform.parent.gameObject.name == SelectUnit.instance.selectedPos.name) return;

        // 타겟 체크
        if (other.gameObject.CompareTag("Enemy") && (!isOnTarget || enemyTrans.GetComponent<EnemyBase>().isDead))
        {
            enemyTrans = other.gameObject.transform;
            isOnTarget = true;
        }
        if(enemyTrans != null) CalculateSpriteRen(enemyTrans);

        // 타겟 공격
        if(other.gameObject.CompareTag("Enemy") && prevAtkSpeed >= limitAtkSpeed)
        {
            prevAtkSpeed = 0f;

            if (heroInfo.attackType == AttackType.근거리)
            {
                anim.SetBool(IsAttacking,true);
                MeleeWeapon meleeWeapon = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, weaponEffect).GetComponent<MeleeWeapon>();
                meleeWeapon.transform.position = enemyTrans.position;
                meleeWeapon.weaponEffect = weaponEffect;
                meleeWeapon.damageType = heroInfo.damageType;
                meleeWeapon.attackDamage = heroInfo.attackDamage;
                meleeWeapon.characterBase = this;
                meleeWeapon.isEnter = false;
            }
            else
            {
                RangeWeapon rangeWeapon = PoolManager.instance.GetPool(PoolManager.instance.weaponEffectPool.queMap, weaponEffect).GetComponent<RangeWeapon>();
                rangeWeapon.weaponEffect = weaponEffect;
                rangeWeapon.damageType = heroInfo.damageType;
                rangeWeapon.attackDamage = heroInfo.attackDamage;
                rangeWeapon.characterBase = this;
                SetLastBulletPos(rangeWeapon.gameObject, enemyTrans,gunPointTrans);
                if(SoundManager.instance.sfxCnt > 10) return;
                SoundManager.instance.SFXPlay(atkSoundType);
            }
        }
    }

    // 타겟 나감
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform == enemyTrans) isOnTarget = false;
        if (heroInfo.attackType == AttackType.근거리) anim.SetBool(IsAttacking, false);
    }

    // 원거리 발사 방향 셋
    private Quaternion CalculateBulletRotation(Transform enemyTrans, Transform startTrans)
    {
        Vector2 bulletDirection = (enemyTrans.position - startTrans.position); // 방향
        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg; // 각도
        Quaternion bulletRotation = Quaternion.AngleAxis(angle, Vector3.forward); // 쿼터니언
        return bulletRotation;
    }
    public void SetLastBulletPos(GameObject bullet,Transform enemyTrans, Transform gunTrans) { bullet.transform.SetPositionAndRotation(gunTrans.position,CalculateBulletRotation(enemyTrans, transform)); }
    
    // 스프라이트 플립
    private void CalculateSpriteRen(Transform enemyTrans) //적을 바라보는 스프라이트렌더러 교체함수
    {  
        Vector2 heroDirection = enemyTrans.position - transform.position;
        if (heroDirection.x > 0) FlipX = heroInfo.heroDefaultSpriteDir == HeroDefaultSpriteDir.Left ? true : false; // Left
        else FlipX = heroInfo.heroDefaultSpriteDir == HeroDefaultSpriteDir.Right ? true : false; // Right
    }

    // 공격 애니메이션이 끝나면 사운드 재생
    public void AtkSound()
    {
        if(SoundManager.instance.sfxCnt > 10) return;
        SoundManager.instance.SFXPlay(atkSoundType);
    }
}
