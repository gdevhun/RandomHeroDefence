using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    private void Awake()
    {
        instance = this;
        Gold = 50;
        Dia = 2;
    }
    
    // 골드
    private int gold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; UpdateCurrencyUI(value, true); }
    }
    [Header ("골드 텍스트")] [SerializeField] private TextMeshProUGUI goldText;

    // 다이아
    private int dia;
    public int Dia
    {
        get { return dia; }
        set { dia = value; UpdateCurrencyUI(value, false); }
    }
    [Header ("다이아 텍스트")] [SerializeField] private TextMeshProUGUI diaText;

    // 재화 얻기
    public void AcquireCurrency(int amount, bool isGold)
    {
        // 골드
        if(isGold) { Gold += amount; return; }

        // 다이아
        Dia += amount;
    }

    // 재화 사용
    public bool ConsumeCurrency(int amount, bool isGold)
    {
        // 골드
        if(isGold)
        {
            if(amount <= gold) { Gold -= amount; return true; }

            SoundManager.instance.SFXPlay(SoundType.NotEnough);
            return false;
        }

        // 다이아
        if(amount <= dia) { Dia -= amount; return true; }

        SoundManager.instance.SFXPlay(SoundType.NotEnough);
        return false;
    }

    // 재화 UI 갱신
    private void UpdateCurrencyUI(int val, bool isGold)
    {
        // 골드
        if(isGold) { goldText.text = val.ToString(); return; }

        // 다이아
        diaText.text = val.ToString();
    }
}
