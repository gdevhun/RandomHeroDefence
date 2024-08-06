using System;
using System.Collections;
using UnityEngine;

// 스킬 UI 정보
[Serializable]
public class AbilityUiInfo
{
    [Header ("스킬 스프라이트")] public Sprite abilitySprite;
    [Header ("스킬 이름")] public string abilityName;
    [Header ("스킬 내용")] public string abilityContent;
}

// 스킬 시전 관리
public class AbilityManage : MonoBehaviour
{
    [Header ("스킬 정보")] public AbilityBase ability;

    // 스태미나
    [Header ("최대 스태미나")] [SerializeField] private int maxStamina;
    private float maxStaminaFloat;
    private int stamina;
    public int Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;

            // 최대 스태미너가 되고 타겟이 존재 할 때 스킬 시전
            if((louizyCnt == 0 && stamina >= maxStamina && characterBase.isOnTarget) || (louizyCnt > 0 && stamina >= maxStamina - 1 && characterBase.isOnTarget))
            {
                if(ability is SyncAbilityBase syncAbilityBase) syncAbilityBase.CastAbility(characterBase);
                else if(ability is AsyncAbilityBase asyncAbilityBase) StartCoroutine(asyncAbilityBase.CastAbility(characterBase));
                stamina = 0;
            }

            UpdateStaminaUI();
        }
    }
    [HideInInspector] public CharacterBase characterBase;
    public static int louizyCnt;
    [Header ("스태미나 필 오브젝트")] [SerializeField] private GameObject staminaFillObj;
    private void Start()
    {
        characterBase = GetComponent<CharacterBase>();
        maxStaminaFloat = maxStamina;

        // 스태미너 O => 스태미너에 따라 스킬 시전
        // 스태미너 X => 바로 스킬 시전
        if(maxStamina > 0) StartCoroutine(StaminaIncreament());
        else
        {
            if(ability is SyncAbilityBase syncAbilityBase) syncAbilityBase.CastAbility(characterBase);
            else if(ability is AsyncAbilityBase asyncAbilityBase) StartCoroutine(asyncAbilityBase.CastAbility(characterBase));
        }
    }
    private void OnEnable()
    {
        // 스태미너 X => 활성화 할 때 마다 스킬 시전
        if(maxStamina > 0 || characterBase == null) return;
        if(ability is SyncAbilityBase syncAbilityBase) syncAbilityBase.CastAbility(characterBase);
        else if(ability is AsyncAbilityBase asyncAbilityBase) StartCoroutine(asyncAbilityBase.CastAbility(characterBase));
    }

    // 스태미나 증가
    private IEnumerator StaminaIncreament()
    {
        while(true)
        {
            ++Stamina;
            yield return StageManager.instance.oneSecond;
        }
    }

    // 스태미나 UI 갱신
    private void UpdateStaminaUI()
    {
        Vector3 curScale = staminaFillObj.transform.localScale;
        curScale.y = Mathf.Lerp(0, 0.68f, Stamina / maxStaminaFloat);
        staminaFillObj.transform.localScale = curScale;
    }

    // 특정 유닛 비활성화 처리
    private void OnDisable()
    {
        if(characterBase == null) return;
        switch(characterBase.heroInfo.unitType)
        {
            case UnitType.솔져 : SellUnit.instance.soldierCnt--;
                break;
            case UnitType.에키온 : EnemyBase.decreaseMoveSpeed -= 0.05f; if(EnemyBase.decreaseMoveSpeed < 0) EnemyBase.decreaseMoveSpeed = 0;
                break;
            case UnitType.뱃 : EnemyBase.decreaseMagDef -= 10f; if(EnemyBase.decreaseMagDef < 0) EnemyBase.decreaseMagDef = 0;
                break;
            case UnitType.바이킹 : EnemyBase.decreasePhyDef -= 10f; if(EnemyBase.decreasePhyDef < 0) EnemyBase.decreasePhyDef = 0;
                break;
            case UnitType.에이든 : UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] += 4; if(UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] > 72) UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] = 72; 
                break;
            case UnitType.알론소 : EnemyBase.increaseEnemyGold -= 10; if(EnemyBase.increaseEnemyGold < 0) EnemyBase.increaseEnemyGold = 0;
                break;
            case UnitType.아아솔 : UpgradeUnit.instance.damageUpgradeMap[DamageType.마법] -= 10; if(UpgradeUnit.instance.damageUpgradeMap[DamageType.마법] < 0) UpgradeUnit.instance.damageUpgradeMap[DamageType.마법] = 0;
                break;
            case UnitType.배니스 : UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] -= 10; if(UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] < 0) UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] = 0;
                break;
            case UnitType.루이지 : louizyCnt--; if(louizyCnt < 0) louizyCnt = 0;
                break;
            case UnitType.막더스 : EnemyBase.decreaseMagDef -= 50f; if(EnemyBase.decreaseMagDef < 0) EnemyBase.decreaseMagDef = 0; EnemyBase.decreasePhyDef -= 50f; if(EnemyBase.decreasePhyDef < 0) EnemyBase.decreasePhyDef = 0;
                break;
            default :
                break;
        }
    }
}
