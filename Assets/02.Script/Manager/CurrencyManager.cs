using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    private void Awake()
    {
        instance = this;
        Gold = 50;
        Dia = 100;
    }
    
    // 골드
    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            UpdateCurrencyUI(value, true);
        }
    }

    // 다이아
    private int dia;
    public int Dia
    {
        get { return dia; }
        set
        {
            dia = value;
            UpdateCurrencyUI(value, false);
        }
    }

    [Header ("골드 텍스트")] [SerializeField] private TextMeshProUGUI goldText;
    [Header ("다이아 텍스트")] [SerializeField] private TextMeshProUGUI diaText;

    // 재화 얻기
    public void AcquireCurrency(int amount, bool isGold)
    {
        if(isGold)
        {
            Gold += amount;
            return;
        }

        Dia += amount;
    }

    // 재화 사용
    public bool ConsumeCurrency(int amount, bool isGold)
    {
        if(isGold)
        {
            if(amount <= gold)
            {
                Gold -= amount;
                return true;
            }

            // 사운드
            SoundManager.instance.SFXPlay(SoundType.NotEnough);
            return false;
        }

        if(amount <= dia)
        {
            Dia -= amount;
            return true;
        }

        // 사운드
        SoundManager.instance.SFXPlay(SoundType.NotEnough);
        return false;
    }

    // 재화 UI 갱신
    private void UpdateCurrencyUI(int val, bool isGold)
    {
        if(isGold)
        {
            goldText.text = val.ToString();
            return;
        }

        diaText.text = val.ToString();
    }
}
