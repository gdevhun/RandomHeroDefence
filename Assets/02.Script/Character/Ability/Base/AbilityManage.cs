using System;
using System.Collections;
using System.Collections.Generic;
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
    [Header ("최대 스태미나")] public int maxStamina;
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
                // 드래그 체크
                if(SelectUnit.instance.isDrag && transform.parent.gameObject.name == SelectUnit.instance.selectedPos.name) return;

                if(ability.abilitySoundType != SoundType.GetUnit) SoundManager.instance.SFXPlay(ability.abilitySoundType);
                if(ability is SyncAbilityBase syncAbilityBase) syncAbilityBase.CastAbility(characterBase);
                else if(ability is AsyncAbilityBase asyncAbilityBase) StartCoroutine(asyncAbilityBase.CastAbility(characterBase));
                stamina = 0;
            }

            UpdateStaminaUI();
        }
    }
    private CharacterBase characterBase;
    public static int louizyCnt;
    [Header ("스태미나 필 오브젝트")] [SerializeField] private GameObject staminaFillObj;

    // 인스턴스화 => OnEnable 까지만 호출됨
    // 소환 => Start 호출됨
    private void Start()
    {
        characterBase = GetComponent<CharacterBase>();
        maxStaminaFloat = maxStamina;

        // 스태미너 O => 소환 될 때 한 번 스태미너에 따라 스킬 시전
        if(maxStamina > 0) StartCoroutine(StaminaIncreament());
        else // 스태미너 X => 소환 될 때 한 번 즉시 시전
        {
            SyncAbilityBase syncAbilityBase = ability as SyncAbilityBase;
            syncAbilityBase.CastAbility(characterBase);
        }

        // 막더스 처리
        if(characterBase.heroInfo.unitType == UnitType.막더스)
        {
            EnemyBase.DecreaseMagDef += 50f;
            EnemyBase.DecreasePhyDef += 50f;
        }
    }
    private void OnEnable()
    {
        if(characterBase == null) return;

        // 스태미너 O => 소환 될 때 한 번 스태미너에 따라 스킬 시전
        if(maxStamina > 0) StartCoroutine(StaminaIncreament());
        else // 스태미너 X => 소환 될 때 한 번 즉시 시전
        {
            SyncAbilityBase syncAbilityBase = ability as SyncAbilityBase;
            syncAbilityBase.CastAbility(characterBase);
        }

        // 막더스 처리
        if(characterBase.heroInfo.unitType == UnitType.막더스)
        {
            EnemyBase.DecreaseMagDef += 50f;
            EnemyBase.DecreasePhyDef += 50f;
        }
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
            case UnitType.에키온 : EnemyBase.DecreaseMoveSpeed -= 0.05f; if(EnemyBase.DecreaseMoveSpeed < 0) EnemyBase.DecreaseMoveSpeed = 0;
                break;
            case UnitType.뱃 : EnemyBase.DecreaseMagDef -= 20f; if(EnemyBase.DecreaseMagDef < 0) EnemyBase.DecreaseMagDef = 0;
                break;
            case UnitType.바이킹 : EnemyBase.DecreasePhyDef -= 20f; if(EnemyBase.DecreasePhyDef < 0) EnemyBase.DecreasePhyDef = 0;
                break;
            case UnitType.에이든 : UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] += 4; if(UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] > 72) UiUnit.instance.unitSpawn.gradeWeightMap[HeroGradeType.일반] = 72; 
                break;
            case UnitType.아아솔 : UpgradeUnit.instance.damageUpgradeMap[DamageType.마법] -= 10; if(UpgradeUnit.instance.damageUpgradeMap[DamageType.마법] < 0) UpgradeUnit.instance.damageUpgradeMap[DamageType.마법] = 0; UiUnit.instance.magText.text = UpgradeUnit.instance.damageUpgradeMap[DamageType.마법].ToString() + " %";
                break;
            case UnitType.배니스 : UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] -= 10; if(UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] < 0) UpgradeUnit.instance.damageUpgradeMap[DamageType.물리] = 0; UiUnit.instance.phyText.text = UpgradeUnit.instance.damageUpgradeMap[DamageType.물리].ToString() + " %";
                break;
            case UnitType.루이지 : louizyCnt--; if(louizyCnt < 0) louizyCnt = 0;
                break;
            case UnitType.막더스 : EnemyBase.DecreaseMagDef -= 50f; if(EnemyBase.DecreaseMagDef < 0) EnemyBase.DecreaseMagDef = 0; EnemyBase.DecreasePhyDef -= 50f; if(EnemyBase.DecreasePhyDef < 0) EnemyBase.DecreasePhyDef = 0;
                break;
            default :
                break;
        }
        if(maxStamina > 0) Stamina = 0;
    }
}
