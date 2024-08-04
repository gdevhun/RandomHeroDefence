using System;
using System.Collections;
using UnityEngine;

// 스킬 정보
[Serializable]
public class AbilityInfo
{
    [Header ("스킬 스프라이트")] public Sprite abilitySprite;
    [Header ("스킬 이름")] public string abilityName;
    [Header ("스킬 내용")] public string abilityContent;
}

public abstract class AbilityBase : MonoBehaviour
{
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
            Debug.Log("현재 스태미나 :" + stamina);

            // 최대 스태미너가 되고 타겟이 존재 할 때 스킬 시전
            if(stamina >= maxStamina && characterBase.isOnTarget)
            {
                CastAbility();
                stamina = 0;
            }

            UpdateStaminaUI();
        }
    }
    [Header ("스태미나 필 오브젝트")] [SerializeField] private GameObject staminaFillObj;

    private CharacterBase characterBase;
    [Header ("스킬 정보")] public AbilityInfo abilityInfo;
    private void Start()
    {
        characterBase = GetComponent<CharacterBase>();
        maxStaminaFloat = maxStamina;

        // 스태미너 O => 스태미너에 따라 스킬 시전
        // 스태미너 X => 바로 스킬 시전
        if(maxStamina > 0) StartCoroutine(StaminaIncreament());
        else CastAbility();
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

    // 비활성화 시 초기화
    // private void OnDisable()
    // {
    //     Debug.Log("초기화");
    //     StopAllCoroutines();
    //     Stamina = 0;
    // }

    // 스태미나 UI 갱신
    private void UpdateStaminaUI()
    {
        Vector3 curScale = staminaFillObj.transform.localScale;
        curScale.y = Mathf.Lerp(0, 0.68f, Stamina / maxStaminaFloat);
        staminaFillObj.transform.localScale = curScale;
    }

    // 스킬 => 각 캐릭터 마다 오버라이딩 구현
    protected abstract void CastAbility();
}
