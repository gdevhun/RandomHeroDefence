using UnityEngine;

public class SelectUnit : MonoBehaviour
{
    public static SelectUnit instance;
    private void Awake() { instance = this; }
    [HideInInspector] public GameObject selectedPos; // 선택된 유닛 스폰 위치
    [Header ("유닛 스폰 위치만 클릭되게")] [SerializeField] private LayerMask posLayerMask;

    private void Update()
    {
        Select();
    }

    // 유닛 스폰 위치 선택
    private void Select()
    {
        // 마우스 왼쪽 클릭 또는 모바일 터치 시
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            // 마우스 클릭 또는 모바일 터치 위치 가져옴
            Vector3 inputPosition = Input.GetMouseButtonDown(0) ? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            inputPosition.z = 0;

            // 히트된 스폰 할 위치 콜라이더 가져와서 선택된 스폰 위치로 설정
            RaycastHit2D hit = Physics2D.Raycast(inputPosition, Vector2.zero, Mathf.Infinity, posLayerMask);

            // 히트된 콜라이더가 있는지 체크
            if(hit.collider == null) return;

            // 선택된 유닛 스폰 위치 저장
            selectedPos = hit.transform.gameObject;
        }
    }
}
