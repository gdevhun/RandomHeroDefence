using UnityEngine;

[System.Serializable]
public class RoulettePieceData
{
	[Header ("이미지")] public Sprite img;
	[Header ("설명")] public string desc;
	[Header ("확률 : 각 아이템의 확률 / 모든 아이템의 확률 총합")] [Range(1, 100)] public int percent = 100;
	[HideInInspector] public int idx, weight; // 인덱스, 가중치
}
