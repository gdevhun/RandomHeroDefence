using UnityEngine;
using UnityEngine.UI;

public class RouletteSpin : MonoBehaviour
{
	[Header ("룰렛")] [SerializeField] private Roulette roulette;
	[Header ("회전 버튼")] [SerializeField] private Button spinBtn;

	private void Awake()
	{
        // 회전 버튼에 이벤트 함수 등록
		spinBtn.onClick.AddListener(()=>
		{
            // 회전 중 버튼 상호작용 막음
			spinBtn.interactable = false;

            // 회전이 끝났을 때 호출할 함수 호출
			roulette.Spin(EndOfSpin);
		});
	}

    // 회전이 끝났을 때 호출할 함수
	private void EndOfSpin(RoulettePieceData selectedData)
	{
        // 회전이 끝나면 버튼 상호작용 활성화
		spinBtn.interactable = true;

        // 회전이 끝나면 선택된 아이템 인덱스와 설명 출력
		Debug.Log($"{selectedData.idx}:{selectedData.desc}");
	}
}
