using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoulettePiece : MonoBehaviour
{
	[Header ("이미지 변경할 칸")] [SerializeField] private Image img;
	[Header ("설명 변경할 칸")] [SerializeField] private TextMeshProUGUI desc;

    // 룰렛 내부 각 아이템 정보 업데이트
	public void UpdatePiece(RoulettePieceData pieceData)
	{
		img.sprite = pieceData.img;
		desc.text = pieceData.desc;
	}
}
