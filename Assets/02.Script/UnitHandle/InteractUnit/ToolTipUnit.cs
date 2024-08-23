using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipUnit : MonoBehaviour
{
    [Header ("유닛 이미지")] [SerializeField] private Image unitImage;
    [Header ("유닛 이름 텍스트")] [SerializeField] private TextMeshProUGUI unitNameText;  
    [Header ("유닛 정보 텍스트")] [SerializeField] private TextMeshProUGUI unitInfoText;  
    [Header ("유닛 데미지 텍스트")] [SerializeField] private TextMeshProUGUI unitDmgText;  
    [Header ("유닛 공격속도 텍스트")] [SerializeField] private TextMeshProUGUI unitSpeedText;
    [Header ("스킬 이미지")] [SerializeField] private Image abilityImage;
    [Header ("스킬 이름 텍스트")] [SerializeField] private TextMeshProUGUI abilityNameText;
    [Header ("스킬 설명 텍스트")] [SerializeField] private TextMeshProUGUI abilityContentText;
    [Header ("히든 스킬 설명 텍스트")] [SerializeField] private TextMeshProUGUI hiddenAbilityContentText;

    // 툴팁 정보 설정
    public void SetToolTip(HeroInfo heroInfo, AbilityUiInfo abilityInfo, AbilityUiInfo hiddenAbilityInfo, CharacterBase characterBase)
    {
        unitImage.sprite = heroInfo.unitSprite;
        unitNameText.text = heroInfo.unitType.ToString();
        unitInfoText.text = $"{heroInfo.heroGradeType} / {heroInfo.damageType} / {heroInfo.attackType}";
        unitDmgText.text = ((int)characterBase.GetApplyAttackDamage(characterBase.heroInfo.attackDamage)).ToString();
        unitSpeedText.text = heroInfo.attackSpeed.ToString();
        abilityImage.sprite = abilityInfo.abilitySprite;
        abilityNameText.text = characterBase.GetComponent<AbilityManage>().maxStamina > 0 ? abilityInfo.abilityName + $" (스태미너 : {characterBase.GetComponent<AbilityManage>().maxStamina})" : abilityInfo.abilityName;
        abilityContentText.text = abilityInfo.abilityContent;
        if(heroInfo.heroGradeType == HeroGradeType.신화)
        {
            hiddenAbilityContentText.gameObject.SetActive(true);
            hiddenAbilityContentText.text = hiddenAbilityInfo.abilityContent;
        }
        else hiddenAbilityContentText.gameObject.SetActive(false);
    }
}
