using UnityEngine;

public class SelectUnit : MonoBehaviour
{
    public static SelectUnit instance;
    private void Awake() { instance = this; }

    [Header ("유닛 툴팁 패널")] [SerializeField] private ToolTipUnit unitToolTipPanel;
    [HideInInspector] public GameObject selectedPos; // 선택된 유닛 스폰 위치
    [Header ("유닛 스폰 위치만 클릭되게")] [SerializeField] private LayerMask posLayerMask;
    private Vector3 sPos = Vector3.zero; // 시작 위치
    private const float dragThreshold = 1f; // 클릭과 드래그를 구분하는 임계 값
    private bool isDrag = false; // 드래그 체크

    private void Update()
    {
        Down();
    }

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
                if(unitToolTipPanel.gameObject.activeSelf) unitToolTipPanel.HandleToolTip(false);
                return;
            }

            // 선택된 유닛 스폰 위치 오브젝트
            selectedPos = hit.transform.gameObject;

            // 시작 위치 백업
            sPos = selectedPos.transform.position;
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
                if(!unitToolTipPanel.gameObject.activeSelf) unitToolTipPanel.HandleToolTip(true);
                unitToolTipPanel.SetToolTip(selectedPos.transform.GetChild(0).GetComponent<CharacterBase>().heroInfo);

                // 판매 및 합성 패널 띄우기

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
        }
    }
}
