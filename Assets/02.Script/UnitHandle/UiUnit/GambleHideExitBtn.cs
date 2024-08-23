using UnityEngine;

public class GambleHideExitBtn : MonoBehaviour
{
    [Header ("룰렛")] [SerializeField] private Roulette roulette;
    [Header ("잭팟")] [SerializeField] private JackPot jackPot;

    // 룰렛 또는 잭팟이 실행중인지 체크
    public void OnBtn(bool isHide)
    {
        if(roulette.isSpin || jackPot.isJackPot) return;
        if(isHide) UiUnit.instance.unitHandlePanel.SetActive(false);
        else UiUnit.instance.unitGamblePanel.SetActive(false);
    }
}
