using TMPro;
using UnityEngine;

public class ToolTipUnit : MonoBehaviour
{
    [Header ("유닛 이름 텍스트")] [SerializeField] private TextMeshProUGUI unitNameText;  
    [Header ("유닛 등급 텍스트")] [SerializeField] private TextMeshProUGUI unitGradeText;  
    [Header ("유닛 데미지 타입 텍스트")] [SerializeField] private TextMeshProUGUI unitDmgText;  
    [Header ("유닛 공격범위 타입 텍스트")] [SerializeField] private TextMeshProUGUI unitAtkText;  

    // 툴팁 정보 설정
    public void SetToolTip(HeroInfo heroInfo)
    {
        unitNameText.text = heroInfo.unitType.ToString();
        unitGradeText.text = heroInfo.heroGradeType.ToString();
        unitDmgText.text = heroInfo.damageType.ToString();
        unitAtkText.text = heroInfo.attackType.ToString();
    }

    // 툴팁 패널 여닫기
    public void HandleToolTip(bool op)
    {
        gameObject.SetActive(op);
    }
}
