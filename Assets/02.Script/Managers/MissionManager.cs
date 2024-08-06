using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissionList
{
    //모으기 미션
    일반수집가, 고급수집가, 희귀수집가, 전설수집가, 신화수집가,
    
    //스테이지 미션
    초보, 중수, 숙련자, 고수, 장인,
    
    //골드 모으기 미션
    부자되기첫걸음, 나는구두쇠, 내가재드래곤,
    
    //다이아 모으기 미션
    전당포사장되기, 금은방사장되기, 나는대부호,
    
    //서브미션
    줄타기장인, 메시급드리블, 갬블러, 룰렛중독, 가챠중독, 도박치료상담전화는1336
}
public class MissionManager : MonoBehaviour
{
    private Dictionary<MissionList,bool> missionStatus;
    
    //아래 3개변수 사용하면됨
    public int summonFailures = 0;  //유닛뽑기 꽝, 실패 횟수
    public int rouletteFailures = 0;  //룰랫돌리기 꽝,실패 횟수
    public int gachaFailures = 0; //신화뽑기(가챠) 꽝,실패 횟수
    #region MyRegion
    
    /*
     * ### 모으기 미션

1. 일반수집가 : 일반 5개 다 모으기 ⇒ 80 골드 획득.
2. 고급수집가 : 고급 5개 다 모으기 ⇒ 200 골드 획득.
3. 희귀수집가 : 희귀 5개 다 모으기 ⇒ 다이아 3 획득.
4. 전설수집가 : 전설 3개 가진 경우 ⇒ 다이아 10 획득.
5. 신화수집가 : 신화 3개 가진 경우 ⇒ 다이아 20 획득.

### 스테이지 미션

총 50 스테이지

1. 초보 : 10 달성 ⇒ 다이아 2 획득
2. 중수 : 20 달성 ⇒ 다이아 4 획득
3. 숙련자 : 30 달성 ⇒ 다이아 8획득
4. 고수 : 40 달성 ⇒ 다이아 10 획득
5. 장인 : 45 달성 ⇒ 다이야 20 획득

### 골드 모으기 미션

현재 들고 있는 골드

1. 부자되기 첫걸음 : 500 원 ⇒ 다이아 10획득
2. 구두쇠 : 1000 원 ⇒ 다이아 20 획득
3. 대부호 : 2000 원 ⇒ 다이아 30획득

### 다이아 모으기 미션

1. 다이아 수집가 : 10개  ⇒ 골드 300획득
2. 다이아 대부호 : 20개 ⇒ 골드 500획득

### 서브미션

정통한국인 : 보스 10초안에 잡기(아무보스나 가능) ⇒ 다이아 5획득

줄타기장인: 현재 존재하는 몬스터 100개 달성  ⇒ 다이아 3획득

갬블러 : 유닛소환 꽝 5번. ⇒ 다이아 10획득

룰렛중독 : 룰렛 꽝 5번. ⇒ 다이아 10획득

가챠중독 : 신화뽑기 꽝 2번. ⇒ 다이아 10획득

도박치료상담전화는1336 : 갬블러, 룰렛, 가챠 중독 다 깨는 경우. ⇒ 다이아 40획득
     */

    #endregion

    void Start()
    {
        //초기화작업
        missionStatus = new Dictionary<MissionList, bool>();
        foreach (MissionList mission in System.Enum.GetValues(typeof(MissionList)))
        {
            missionStatus[mission] = false;
        }
    }

    void Update()
    {
        CheckCollectionMissions();
        CheckStageMissions();
        CheckGoldMissions();
        CheckDiamondMissions();
        CheckSubMissions();
    }
    void CheckCollectionMissions() //모으기미션
    {
        if (!missionStatus[MissionList.일반수집가] && HasAllItems("General"))
        {
            missionStatus[MissionList.일반수집가] = true;
            CurrencyManager.instance.AcquireCurrency(80, true);
            Debug.Log("일반수집가 퀘 완 80골드 획득");
        }
        if (!missionStatus[MissionList.고급수집가] && HasAllItems("Advanced"))
        {
            missionStatus[MissionList.고급수집가] = true;
            CurrencyManager.instance.AcquireCurrency(200, true);
            Debug.Log("고급수집가 퀘 완 200골드 획득");
        }
        if (!missionStatus[MissionList.희귀수집가] && HasAllItems("General"))
        {
            missionStatus[MissionList.희귀수집가] = true;
            CurrencyManager.instance.AcquireCurrency(3, false);
            Debug.Log("희귀수집가 퀘 완 3다이아 획득");
        }
        if (!missionStatus[MissionList.전설수집가] && HasAllItems("Legend"))
        {
            missionStatus[MissionList.전설수집가] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            Debug.Log("전설수집가 퀘 완 10다이아 획득");
        }
        if (!missionStatus[MissionList.신화수집가] && HasAllItems("Myth"))
        {
            missionStatus[MissionList.신화수집가] = true;
            CurrencyManager.instance.AcquireCurrency(20, false);
            Debug.Log("신화수집가 퀘 완 20다이아 획득");
        }
    }

    void CheckStageMissions() //스테이지미션
    {
        if (!missionStatus[MissionList.초보] && StageManager.instance.CurStage >= 10)
        {
            missionStatus[MissionList.초보] = true;
            CurrencyManager.instance.AcquireCurrency(2, false);
            Debug.Log("초보 퀘 완 2다이아 획득");
        }
        if (!missionStatus[MissionList.중수] && StageManager.instance.CurStage >= 20)
        {
            missionStatus[MissionList.중수] = true;
            CurrencyManager.instance.AcquireCurrency(4, false);
            Debug.Log("중수 퀘 완 4다이아 획득");
        }
        if (!missionStatus[MissionList.숙련자] && StageManager.instance.CurStage >= 30)
        {
            missionStatus[MissionList.숙련자] = true;
            CurrencyManager.instance.AcquireCurrency(8, false);
            Debug.Log("중수 퀘 완 8다이아 획득");
        }
        if (!missionStatus[MissionList.고수] && StageManager.instance.CurStage >= 40)
        {
            missionStatus[MissionList.고수] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            Debug.Log("중수 퀘 완 10다이아 획득");
        }
        if (!missionStatus[MissionList.장인] && StageManager.instance.CurStage >= 45)
        {
            missionStatus[MissionList.장인] = true;
            CurrencyManager.instance.AcquireCurrency(20, false);
            Debug.Log("중수 퀘 완 20다이아 획득");
        }
    }

    void CheckGoldMissions() //골드 모으기 미션
    {
        if (!missionStatus[MissionList.부자되기첫걸음] && CurrencyManager.instance.Gold >= 500)
        {
            missionStatus[MissionList.부자되기첫걸음] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            Debug.Log("부자되기 첫걸음 퀘 완 10 다이아 획득");
        }
        if (!missionStatus[MissionList.나는구두쇠] && CurrencyManager.instance.Gold >= 1000)
        {
            missionStatus[MissionList.나는구두쇠] = true;
            CurrencyManager.instance.AcquireCurrency(20, false);
            Debug.Log("구두쇠 퀘 완 20 다이아 획득");
        }
        if (!missionStatus[MissionList.내가재드래곤] && CurrencyManager.instance.Gold >= 2000)
        {
            missionStatus[MissionList.내가재드래곤] = true;
            CurrencyManager.instance.AcquireCurrency(30, false);
            Debug.Log("재드래곤 퀘 완 30 다이아 획득");
        }
    }

    void CheckDiamondMissions() //다이아 모으기 미션
    {
        if (!missionStatus[MissionList.전당포사장되기] && CurrencyManager.instance.Dia >= 10)
        {
            missionStatus[MissionList.전당포사장되기] = true;
            CurrencyManager.instance.AcquireCurrency(300, true);
            Debug.Log("전당포사장 퀘 완 300골드 획득");
        }
        if (!missionStatus[MissionList.금은방사장되기] && CurrencyManager.instance.Dia >= 20)
        {
            missionStatus[MissionList.금은방사장되기] = true;
            CurrencyManager.instance.AcquireCurrency(500, true);
            Debug.Log("금은방사장 퀘 완 500골드 획득");
        }
        if (!missionStatus[MissionList.나는대부호] && CurrencyManager.instance.Dia >= 40)
        {
            missionStatus[MissionList.나는대부호] = true;
            CurrencyManager.instance.AcquireCurrency(1000, true);
            Debug.Log("나는대부호 퀘 완 500골드 획득");
        }
    }

    void CheckSubMissions()
    {
        if (!missionStatus[MissionList.줄타기장인] && StageManager.instance.EnemyCnt >= 100)
        {
            missionStatus[MissionList.줄타기장인] = true;
            CurrencyManager.instance.AcquireCurrency(5, false);
            Debug.Log("줄타기장인 퀘 완");
        }
        if (!missionStatus[MissionList.메시급드리블] && StageManager.instance.EnemyCnt >= 110)
        {
            missionStatus[MissionList.메시급드리블] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            Debug.Log("메시급드리블 퀘 완");
        }
        if (!missionStatus[MissionList.갬블러] && summonFailures >= 5)
        {
            missionStatus[MissionList.갬블러] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            Debug.Log("갬블러 퀘 완");
        }
        if (!missionStatus[MissionList.룰렛중독] && rouletteFailures >= 5)
        {
            missionStatus[MissionList.룰렛중독] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            Debug.Log("룰렛 중독 퀘 완");
        }
        if (!missionStatus[MissionList.가챠중독] && gachaFailures >= 3)
        {
            missionStatus[MissionList.가챠중독] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            Debug.Log("가챠중독 퀘 완");
        }
        if (!missionStatus[MissionList.도박치료상담전화는1336] && 
            missionStatus[MissionList.갬블러] && 
            missionStatus[MissionList.룰렛중독] && 
            missionStatus[MissionList.가챠중독])
        {
            missionStatus[MissionList.도박치료상담전화는1336] = true;
            CurrencyManager.instance.AcquireCurrency(50, false);
            Debug.Log("도박치료상단전화퀘 완");
        }
    }

    bool HasAllItems(string rarity)
    {
        //필드에 해당 영웅이 존재하는지에 대한 함수 처리
        return true;
    }

}
