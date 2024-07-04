using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Roulette : MonoBehaviour
{
	[Header ("피스, 라인 프리팹")] [SerializeField] private Transform piecePref, linePref, pieceParent, lineParent;
	[Header ("각 아이템 정보 관리")] [SerializeField] private List<RoulettePieceData> pieceInfoList = new List<RoulettePieceData>();
	[Header ("회전 시간")] [SerializeField] private int spinDuration;
	[Header ("룰렛판")] [SerializeField] private Transform spinningRoulette;
	[Header ("회전 애니메이션 커브")] [SerializeField] private AnimationCurve spinningCurve;

	private	float pieceAngle, halfPieceAngle, halfPieceAngleWithPaddings; // 룰렛 내부에 아이템 하나가 배치되는 각도, 룰렛 내부에 아이템 하나가 배치되는 각도의 절반, 룰렛 내부에 아이템 하나가 배치되는 각도의 절반에 패딩 적용
	private	int	accumulatedWeight; // 가중치 계산용
	private	bool isSpin = false; // 회전 중 인지 체크
	private	int	selectedIdx = 0; // 선택된 아이템 인덱스

	private void Awake()
	{
        // 룰렛 내부 아이템 배치 각도 계산
		pieceAngle = 360 / pieceInfoList.Count;
		halfPieceAngle = pieceAngle * 0.5f;
		halfPieceAngleWithPaddings = halfPieceAngle - (halfPieceAngle * 0.25f);

        // 룰렛 내부 아이템 및 선 배치
		SpawnPiecesAndLines();

        // 각 아이템의 가중치와 인덱스 계산
		CalculateWeightsAndIndices();
	}

    // 룰렛 내부 아이템 및 선 배치
	private void SpawnPiecesAndLines()
	{
		for(int i = 0; i < pieceInfoList.Count; i++)
		{
            // 아이템 생성
			Transform piece = Instantiate(piecePref, pieceParent.position, Quaternion.identity, pieceParent);

            // 아이템 정보 셋
			piece.GetComponent<RoulettePiece>().UpdatePiece(pieceInfoList[i]);

            // 룰렛 아이템 회전
			piece.RotateAround(pieceParent.position, Vector3.back, pieceAngle * i);

            // 선 생성
			Transform line = Instantiate(linePref, lineParent.position, Quaternion.identity, lineParent);

            // 룰렛 선 회전
			line.RotateAround(lineParent.position, Vector3.back, (pieceAngle * i) + halfPieceAngle);
		}
	}

    // 룰렛 내부 각 아이템의 가중치와 인덱스 계산
	private void CalculateWeightsAndIndices()
	{
		for(int i = 0; i < pieceInfoList.Count; i++)
		{
            // 현재 아이템 인덱스 셋
			pieceInfoList[i].idx = i;

            // 확률이 0 이하면 확률 1로 셋
			if(pieceInfoList[i].percent <= 0) pieceInfoList[i].percent = 1;

            // 가중치에 현재 아이템의 확률 누적
			accumulatedWeight += pieceInfoList[i].percent;

            // 현재 아이템의 가중치를 현재 누적된 가중치로 셋
			pieceInfoList[i].weight = accumulatedWeight;
		}
	}

    // 아이템 선택
	private int GetRandomIndex()
	{
        // 0 ~ 누적된 가중치 사이 랜덤값 뽑기
		int weight = Random.Range(0, accumulatedWeight);

        // 선택된 아이템 인덱스 반환
		for(int i = 0; i < pieceInfoList.Count; i++) if (pieceInfoList[i].weight > weight) return i;

		return 0;
	}

    // 룰렛 회전
	public void Spin(UnityAction<RoulettePieceData> action=null)
	{
        // 이미 회전 중 인지 체크
		if(isSpin) return;

        // 룰렛의 결과로 선택된 아이템 인덱스 가져옴
		selectedIdx = GetRandomIndex();

        // 선택된 아이템의 중심 각도 계산
		float angle = pieceAngle * selectedIdx;

        // 선택된 아이템 범위 내 임의 각도 계산
		float leftOffset = (angle - halfPieceAngleWithPaddings) % 360;
		float rightOffset = (angle + halfPieceAngleWithPaddings) % 360;
		float randomAngle = Random.Range(leftOffset, rightOffset);

        // 목표각도 = 계산한 임의 각도 + 360 * 회전 시간 * 회전 속도
		int rotateSpeed	= 2;
		float targetAngle = randomAngle + 360 * spinDuration * rotateSpeed;

        // 회전 중
		isSpin = true;
        
        // 목표각도 까지 회전
		StartCoroutine(OnSpin(targetAngle, action));
	}

    // 목표각도 까지 회전
	private IEnumerator OnSpin(float end, UnityAction<RoulettePieceData> action)
	{
		float current = 0;
		float percent = 0;

		while(percent < 1)
		{
			current += Time.deltaTime;
			percent = current / spinDuration;

			float z = Mathf.Lerp(0, end, spinningCurve.Evaluate(percent));
			spinningRoulette.rotation = Quaternion.Euler(0, 0, z);

			yield return null;
		}

        // 회전 종료
		isSpin = false;

        // 룰렛 회전이 종료되었을때 호출 할 함수들을 등록해둔 함수 호출
		if(action != null) action.Invoke(pieceInfoList[selectedIdx]);
	}
}
