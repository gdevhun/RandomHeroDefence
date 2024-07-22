using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    private void Awake()
    {
        instance = this;
        Gold = 50;
        Dia = 10;
    }
    
    // 골드
    private int gold;
    public int Gold
    {
        get { return gold; }
        set
        {
            gold = value;
            UpdateCurrencyUI();
            Debug.Log("현재 골드 : " + gold);
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
            UpdateCurrencyUI();
            Debug.Log("현재 다이아 : " + dia);
        }
    }

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
            return false;
        }

        if(amount <= dia)
        {
            Dia -= amount;
            return true;
        }
        return false;
    }

    // 재화 UI 갱신
    private void UpdateCurrencyUI() {}
}
