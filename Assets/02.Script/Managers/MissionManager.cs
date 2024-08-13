using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
[System.Serializable]
public class MissionText
{
    public string missionInfo;
    public GameObject missionObject;
    public string missionText;

    public MissionText(GameObject missionObject, string missionText)
    {
        this.missionObject = missionObject;
        this.missionText = missionText;
    }
}
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
    private Dictionary<MissionList,bool> missionStatus;
    public List<MissionText> missionTextList = new List<MissionText>();
    public Image missionClearImg;
    
    //아래 3개변수 사용하면됨
    [HideInInspector] public int summonFailures = 0;  //유닛뽑기 꽝, 실패 횟수
    [HideInInspector] public int rouletteFailures = 0;  //룰랫돌리기 꽝,실패 횟수
    [HideInInspector] public int gachaFailures = 0; //신화뽑기(가챠) 꽝,실패 횟수

    [SerializeField] private RectTransform missionListPanel;
    private readonly Vector2 targetPanelRect = new Vector3(960, 490);
    private readonly Vector2 invisiblePanelRect = new Vector3(1460, 490);
    private readonly WaitForSeconds twoSec = new WaitForSeconds(2f);
    private bool isRoutineRunning; //코루틴 중복 호출을 위한 변수
    private void Awake() { instance = this; }
    void Start()
    {
        //초기화작업
        missionStatus = new Dictionary<MissionList, bool>();
        foreach (MissionList mission in System.Enum.GetValues(typeof(MissionList)))
        {
            missionStatus[mission] = false;
        }
    }
    
    public void OpenMissionListBtn()
    {
        missionListPanel.localPosition = targetPanelRect;
        SoundManager.instance.SFXPlay(SoundType.Click);
    }
    public void CloseMissionListBtn() => missionListPanel.localPosition = invisiblePanelRect;

    private IEnumerator NotifyMissionClear()
    {   //미션 완료를 UI표시하는 코루틴함수
        if (isRoutineRunning) yield break; // 이미 실행 중이라면 중지

        isRoutineRunning = true;
        missionClearImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f); 
        missionClearImg.gameObject.SetActive(false);

        isRoutineRunning = false;
    }
    private void UpdateMissionInfo(int listNum)
    {   //미션 완료시 text를 업데이트하기 위해 자식들을 가져와 수정
        Transform imageChild = missionTextList[listNum].missionObject.transform.GetChild(1);
        Transform textChild = missionTextList[listNum].missionObject.transform.GetChild(3);
        if (imageChild.TryGetComponent(out Image inImage))
        {
            inImage.gameObject.SetActive(true);
        }
        if (textChild.TryGetComponent(out TextMeshProUGUI inTextMeshProUGUI))
        {
            inTextMeshProUGUI.text = missionTextList[listNum].missionText;
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
    private void CheckCollectionMissions() //모으기미션
    {
        if (!missionStatus[MissionList.일반수집가] && HasAllItems(HeroGradeType.일반))
        {
            missionStatus[MissionList.일반수집가] = true;
            CurrencyManager.instance.AcquireCurrency(50, true);
            UpdateMissionInfo(0);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.고급수집가] && HasAllItems(HeroGradeType.고급))
        {
            missionStatus[MissionList.고급수집가] = true;
            CurrencyManager.instance.AcquireCurrency(150, true);
            UpdateMissionInfo(1);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.희귀수집가] && HasAllItems(HeroGradeType.희귀))
        {
            missionStatus[MissionList.희귀수집가] = true;
            CurrencyManager.instance.AcquireCurrency(2, false);
            UpdateMissionInfo(2);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.전설수집가] && HasAllItems(HeroGradeType.전설))
        {
            missionStatus[MissionList.전설수집가] = true;
            CurrencyManager.instance.AcquireCurrency(4, false);
            UpdateMissionInfo(3);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.신화수집가] && HasAllItems(HeroGradeType.신화))
        {
            missionStatus[MissionList.신화수집가] = true;
            CurrencyManager.instance.AcquireCurrency(6, false);
            UpdateMissionInfo(4);
            StartCoroutine(NotifyMissionClear());
        }
    }

    private void CheckStageMissions() //스테이지미션
    {
        if (!missionStatus[MissionList.초보] && StageManager.instance.CurStage >= 10)
        {
            missionStatus[MissionList.초보] = true;
            CurrencyManager.instance.AcquireCurrency(2, false);
            UpdateMissionInfo(5);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.중수] && StageManager.instance.CurStage >= 20)
        {
            missionStatus[MissionList.중수] = true;
            CurrencyManager.instance.AcquireCurrency(4, false);
            UpdateMissionInfo(6);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.숙련자] && StageManager.instance.CurStage >= 30)
        {
            missionStatus[MissionList.숙련자] = true;
            CurrencyManager.instance.AcquireCurrency(8, false);
            UpdateMissionInfo(7);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.고수] && StageManager.instance.CurStage >= 40)
        {
            missionStatus[MissionList.고수] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            UpdateMissionInfo(8);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.장인] && StageManager.instance.CurStage >= 45)
        {
            missionStatus[MissionList.장인] = true;
            CurrencyManager.instance.AcquireCurrency(20, false);
            UpdateMissionInfo(9);
            StartCoroutine(NotifyMissionClear());
        }
    }

    private void CheckGoldMissions() //골드 모으기 미션
    {
        if (!missionStatus[MissionList.부자되기첫걸음] && CurrencyManager.instance.Gold >= 5000)
        {
            missionStatus[MissionList.부자되기첫걸음] = true;
            CurrencyManager.instance.AcquireCurrency(5, false);
            UpdateMissionInfo(10);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.나는구두쇠] && CurrencyManager.instance.Gold >= 20000)
        {
            missionStatus[MissionList.나는구두쇠] = true;
            CurrencyManager.instance.AcquireCurrency(15,false);
            UpdateMissionInfo(11);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.내가재드래곤] && CurrencyManager.instance.Gold >= 50000)
        {
            missionStatus[MissionList.내가재드래곤] = true;
            CurrencyManager.instance.AcquireCurrency(20, false);
            UpdateMissionInfo(12);
            StartCoroutine(NotifyMissionClear());
        }
    }

    private void CheckDiamondMissions() //다이아 모으기 미션
    {
        if (!missionStatus[MissionList.전당포사장되기] && CurrencyManager.instance.Dia >= 5)
        {
            missionStatus[MissionList.전당포사장되기] = true;
            CurrencyManager.instance.AcquireCurrency(200, true);
            UpdateMissionInfo(13);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.금은방사장되기] && CurrencyManager.instance.Dia >= 10)
        {
            missionStatus[MissionList.금은방사장되기] = true;
            CurrencyManager.instance.AcquireCurrency(500, true);
            UpdateMissionInfo(14);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.나는대부호] && CurrencyManager.instance.Dia >= 20)
        {
            missionStatus[MissionList.나는대부호] = true;
            CurrencyManager.instance.AcquireCurrency(1000, true);
            UpdateMissionInfo(15);
            StartCoroutine(NotifyMissionClear());
        }
    }

    private void CheckSubMissions()
    {
        if (!missionStatus[MissionList.줄타기장인] && StageManager.instance.EnemyCnt >= 100)
        {
            missionStatus[MissionList.줄타기장인] = true;
            CurrencyManager.instance.AcquireCurrency(5, false);
            UpdateMissionInfo(16);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.메시급드리블] && StageManager.instance.EnemyCnt >= 110)
        {
            missionStatus[MissionList.메시급드리블] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            UpdateMissionInfo(17);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.갬블러] && summonFailures >= 5)
        {
            missionStatus[MissionList.갬블러] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            UpdateMissionInfo(18);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.룰렛중독] && rouletteFailures >= 5)
        {
            missionStatus[MissionList.룰렛중독] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            UpdateMissionInfo(19);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.가챠중독] && gachaFailures >= 3)
        {
            missionStatus[MissionList.가챠중독] = true;
            CurrencyManager.instance.AcquireCurrency(10, false);
            UpdateMissionInfo(20);
            StartCoroutine(NotifyMissionClear());
        }
        if (!missionStatus[MissionList.도박치료상담전화는1336] && 
            missionStatus[MissionList.갬블러] && 
            missionStatus[MissionList.룰렛중독] && 
            missionStatus[MissionList.가챠중독])
        {
            missionStatus[MissionList.도박치료상담전화는1336] = true;
            CurrencyManager.instance.AcquireCurrency(25, false);
            UpdateMissionInfo(21);
            StartCoroutine(NotifyMissionClear());
        }
    }

    private bool HasAllItems(HeroGradeType heroGradeType)
    {
        //필드에 해당 영웅이 존재하는지에 대한 함수 처리
        Dictionary<UnitType, int> missionUnitMap = new Dictionary<UnitType, int>(); // 미션 유닛 맵핑
        for(int i = 0; i < 5; i++)
        {
            for(int j = 0; j < GetUnitBase.unitPosMap[(UnitType)(5 * (int)heroGradeType + i)].Count; j++)
            {
                // 유닛 위치 가져옴
                GameObject unitPos = GetUnitBase.unitPosMap[(UnitType)(5 * (int)heroGradeType + i)].ElementAt(j).Key;

                // 유닛 체크
                if(unitPos.transform.childCount == 0) continue;

                // 유닛 타입 가져옴
                UnitType unitType = unitPos.transform.GetChild(0).GetComponent<CharacterBase>().heroInfo.unitType;

                // 맵핑 체크
                if(missionUnitMap.ContainsKey(unitType)) continue;

                // 유닛 맵핑
                missionUnitMap.Add(unitType, 1);
            }
        }

        // 유미 히든 스킬
        if(heroGradeType == HeroGradeType.전설 && missionUnitMap.Count == 5)
        {
            // 유미 소환
            MythicUnit.instance.SelectMythic("유미");
            GameObject instantMyth = MythicUnit.instance.GetUnit(null);
            GameObject mythPos = MythicUnit.instance.GetUnitPos(MythicUnit.instance.SelectedMythic);
            instantMyth.transform.SetParent(mythPos.transform);
            instantMyth.transform.localPosition = new Vector3(0, 0.2f, 0);
            ++GetUnitBase.CurUnit;

            // 사운드
            SoundManager.instance.SFXPlay(SoundType.GetUnit);
        }

        return missionUnitMap.Count == 5;
    }

}
