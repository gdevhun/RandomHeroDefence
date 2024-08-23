using UnityEngine;

public class SelectUnit : MonoBehaviour
{
    public static SelectUnit instance;
    private void Awake() { instance = this; }
    [HideInInspector] public GameObject selectedPos; // 선택된 유닛 스폰 위치
    [Header ("유닛 스폰 위치만 클릭되게")] [SerializeField] private LayerMask posLayerMask;
    private Vector3 sPos = Vector3.zero; // 시작 위치
    private const float dragThreshold = 0.5f; // 클릭과 드래그를 구분하는 임계 값
    [HideInInspector] public bool isDrag = false; // 드래그 체크
    [Header ("이동 할 위치 표시")] [SerializeField] private GameObject targetPos;
    [Header ("마우스 클릭 커서 이미지")] [SerializeField] private Texture2D clickCursorImg;

    private void Update() { Down(); }

    // 마우스 클릭
    private void Down()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // 클릭 위치
            Vector3 inputPosition = Input.GetMouseButtonDown(0) ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            inputPosition.z = 0;

            // 히트된 콜라이더 가져와서
            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero, Mathf.Infinity, posLayerMask);

            // 히트된 콜라이더가 있는지 체크
            // 유닛이 있는지 체크
            if(hit.collider == null || hit.transform.childCount < 1)
            {
                // 툴팁 켜져있으면 꺼줌
                if(UiUnit.instance.toolTipPanel.gameObject.gameObject.activeSelf && !UiUnit.instance.unitSellCompPanel.activeSelf) UiUnit.instance.ExitPanel(UiUnit.instance.toolTipPanel.gameObject);
                return;
            }

            // 사정거리 표시 끔
            OnOffIndicateAttackRange(false);

            // 선택된 유닛 스폰 위치 오브젝트
            selectedPos = hit.transform.gameObject;

            // 시작 위치 백업
            sPos = selectedPos.transform.position;

            // 사운드
            SoundManager.instance.SFXPlay(SoundType.Click);

            // 마우스 커서 변경
            Cursor.SetCursor(clickCursorImg, Vector2.zero, CursorMode.ForceSoftware);
        }

        // 시작 위치가 있는지 체크
        if(sPos == Vector3.zero) return;

        // 마우스 드래그
        Drag();

        // 마우스 업
        Up();
    }

    // 마우스 드래그
    private void Drag()
    {
        if(Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            // 현재 위치 갱신
            Vector3 cPos = Input.GetMouseButton(0) ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            cPos.z = 0;
            selectedPos.transform.position = cPos;
            
            // 임계 값 벗어나면 드래그
            isDrag = Vector3.Distance(sPos, cPos) > dragThreshold ? true : false;

            // 히트된 콜라이더 가져와서
            RaycastHit2D hit = Physics2D.Raycast(selectedPos.transform.position, Vector2.zero, Mathf.Infinity, posLayerMask);

            // 이동 할 위치 표시
            if(hit.collider != null)
            {
                targetPos.SetActive(true);
                targetPos.transform.position = hit.collider == selectedPos.GetComponent<Collider2D>() ? sPos : hit.transform.position;
            }
        }
    }

    // 마우스 업
    private void Up()
    {
        if(Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            // 클릭
            if(!isDrag)
            {
                // 다시 시작 위치로
                selectedPos.transform.position = sPos;

                // 시작 위치 초기화
                sPos = Vector3.zero;

                // 유닛 툴팁 띄우기
                if(!UiUnit.instance.toolTipPanel.gameObject.activeSelf) UiUnit.instance.OpenPanel(UiUnit.instance.toolTipPanel.gameObject);
                UiUnit.instance.toolTipPanel.SetToolTip(selectedPos.transform.GetChild(0).GetComponent<CharacterBase>().heroInfo,
                    selectedPos.transform.GetChild(0).GetComponent<AbilityManage>().ability.abilityUiInfo,
                        selectedPos.transform.GetChild(0).GetComponent<CharacterBase>().heroInfo.heroGradeType == HeroGradeType.신화 ? (selectedPos.transform.GetChild(0).GetComponent<AbilityManage>().ability as IHiddenAbility).HiddenAbilityUiInfo : null,
                            selectedPos.transform.GetChild(0).GetComponent<CharacterBase>());

                // 사정거리 표시
                OnOffIndicateAttackRange(true);

                // 이동 할 위치 표시 끔
                targetPos.SetActive(false);

                // 마우스 커서 변경
                Cursor.SetCursor(SceneCtrlManager.instance.cursorImg, Vector2.zero, CursorMode.ForceSoftware);

                // 판매 및 합성 패널 띄우기
                HeroGradeType selectedHeroGradeType = selectedPos.transform.GetChild(0).GetComponent<CharacterBase>().heroInfo.heroGradeType;
                if(selectedHeroGradeType == HeroGradeType.신화) // 신화는 판매 및 합성 패널 켜져있으면 꺼줌
                {
                    if(UiUnit.instance.unitSellCompPanel.activeSelf) UiUnit.instance.ExitPanel(UiUnit.instance.unitSellCompPanel);
                    return;
                }
                UiUnit.instance.OpenPanel(UiUnit.instance.unitSellCompPanel);
                UiUnit.instance.unitSellGoldImage.SetActive(selectedHeroGradeType == HeroGradeType.일반 || selectedHeroGradeType == HeroGradeType.고급);
                UiUnit.instance.unitSellDiaImage.SetActive(selectedHeroGradeType == HeroGradeType.희귀 || selectedHeroGradeType == HeroGradeType.전설);
                if(selectedHeroGradeType == HeroGradeType.일반 || selectedHeroGradeType == HeroGradeType.고급) UiUnit.instance.unitSellGoldText.text = SellUnit.instance.soldierCnt > 0 ? (2 * (20 + 20 * (int)selectedHeroGradeType)).ToString() : (20 + 20 * (int)selectedHeroGradeType).ToString();
                else if(selectedHeroGradeType == HeroGradeType.희귀 || selectedHeroGradeType == HeroGradeType.전설) UiUnit.instance.unitSellDiaText.text = ((int)selectedHeroGradeType).ToString();

                return;
            }

            // 드래그

            // 히트된 콜라이더 가져와서
            RaycastHit2D hit = Physics2D.Raycast(selectedPos.transform.position, Vector2.zero, Mathf.Infinity, posLayerMask);

            // 히트된 콜라이더가 자신 => 경계 벗어남
            if(hit.collider == selectedPos.GetComponent<Collider2D>())
            {
                // 다시 시작 위치로
                selectedPos.transform.position = sPos;

                // 시작 위치 초기화
                sPos = Vector3.zero;

                // 드래그 종료
                isDrag = false;

                // 이동 할 위치 표시 끔
                targetPos.SetActive(false);

                // 마우스 커서 변경
                Cursor.SetCursor(SceneCtrlManager.instance.cursorImg, Vector2.zero, CursorMode.ForceSoftware);

                return;
            }

            // 히트된 콜라이더가 있는 경우

            // 목표 위치로
            selectedPos.transform.position = hit.transform.position;

            // 목표 위치 오브젝트는 시작 위치로
            hit.transform.position = sPos;

            // 시작 위치 초기화
            sPos = Vector3.zero;

            // 드래그 종료
            isDrag = false;

            // 이동 할 위치 표시 끔
            targetPos.SetActive(false);

            // 마우스 커서 변경
            Cursor.SetCursor(SceneCtrlManager.instance.cursorImg, Vector2.zero, CursorMode.ForceSoftware);
        }
    }

    // 사정거리 표시 켜고 끄기
    public void OnOffIndicateAttackRange(bool isOn)
    {
        if(selectedPos == null) return;
        for(int i = 0; i < selectedPos.transform.childCount; i++) selectedPos.transform.GetChild(i).GetComponent<CharacterBase>().indicateAttackRange.SetActive(isOn);
    }
}
