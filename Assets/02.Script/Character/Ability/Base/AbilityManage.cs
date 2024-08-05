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
            if(stamina >= maxStamina && characterBase.isOnTarget)
            {
                if(ability is SyncAbilityBase syncAbilityBase) syncAbilityBase.CastAbility(characterBase);
                else if(ability is AsyncAbilityBase asyncAbilityBase) StartCoroutine(asyncAbilityBase.CastAbility(characterBase));
                stamina = 0;
            }

            UpdateStaminaUI();
        }
    }
    [HideInInspector] public CharacterBase characterBase;
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
}
