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

    // 툴팁 정보 설정
    public void SetToolTip(HeroInfo heroInfo)
    {
        unitImage.sprite = heroInfo.unitSprite;
        unitNameText.text = heroInfo.unitType.ToString();
        unitInfoText.text = $"{heroInfo.heroGradeType} / {heroInfo.damageType} / {heroInfo.attackType}";
        unitDmgText.text = heroInfo.attackDamage.ToString();
        unitSpeedText.text = heroInfo.attackSpeed.ToString();
    }
}
