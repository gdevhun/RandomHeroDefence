using UnityEngine;
using UnityEngine.UI;

public class RouletteSpin : MonoBehaviour, IConsumable
{
	[Header ("룰렛")] [SerializeField] private Roulette roulette;
	[Header ("회전 버튼")] [SerializeField] private Button spinBtn;
	private GameObject rouletteSound; //  룰렛 사운드

	private void Awake()
	{
		amount = 2;

        // 회전 버튼에 이벤트 함수 등록
		spinBtn.onClick.AddListener(()=>
		{
			// 재화 체크
			if(!ConsumeCurrency())
			{
				SoundManager.instance.SFXPlay(SoundType.NotEnough);
				return;
			}

            // 회전 중 버튼 상호작용 막음
			spinBtn.interactable = false;

            // 회전이 끝났을 때 호출할 함수 호출
			roulette.Spin(EndOfSpin);

			// 사운드
			SoundManager.instance.SFXPlay(SoundType.Click);
			rouletteSound = PoolManager.instance.GetPool(PoolManager.instance.soundPool.queMap, SoundType.Roulette);
			rouletteSound.GetComponent<AudioSource>().volume = SoundManager.instance.sfxVolume;
		});
	}

    // 회전이 끝났을 때 호출할 함수
	private void EndOfSpin(RoulettePieceData selectedData)
	{
        // 회전이 끝나면 버튼 상호작용 활성화
		spinBtn.interactable = true;

        // 회전이 끝나면 선택된 배수 만큼 다이아 추가
		int coef = 0;
		bool isSuccess = int.TryParse(selectedData.desc[selectedData.desc.Length - 1].ToString(), out coef);
		CurrencyManager.instance.Dia += 2 * coef;

		// 회전이 끝나면 사운드 반환
		PoolManager.instance.ReturnPool(PoolManager.instance.soundPool.queMap, rouletteSound, SoundType.Roulette);
		if(isSuccess) SoundManager.instance.SFXPlay(SoundType.GetUnit);
		else SoundManager.instance.SFXPlay(SoundType.NotEnough);
	}

    // 재화
    public int amount { get; set; }
    public bool ConsumeCurrency() { return CurrencyManager.instance.ConsumeCurrency(amount, false); }
}
