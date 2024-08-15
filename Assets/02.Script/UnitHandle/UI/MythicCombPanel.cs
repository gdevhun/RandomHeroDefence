using UnityEngine;

public class MythicCombPanel : MonoBehaviour
{
    // 신화 조합 가능 체크 이미지 처리
    private void OnEnable() { MythicUnit.instance.CheckMythicComb(); }
}
