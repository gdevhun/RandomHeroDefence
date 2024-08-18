using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 재생할 배경음 타입 => 키로 사용
public enum BgmType
{
    게임메뉴,
    구간1에서9, 스테이지10,
    구간11에서19, 스테이지20,
    구간21에서29, 스테이지30,
    구간31에서34, 구간35에서39, 스테이지40,
    구간41에서44, 구간45에서49, 스테이지50
}

public class SoundManager : MonoBehaviour
{
    [Header ("BGM 오디오소스")] [SerializeField] private AudioSource bgmSound;
	[Header ("BGM 클립 리스트")] [SerializeField] private AudioClip[] bgmList;
	private Dictionary<BgmType, AudioClip> mapBgm = new Dictionary<BgmType, AudioClip>(); // (타입, 배경음) 맵핑
	[HideInInspector] public float bgmVolume, sfxVolume; // 배경음 볼륨 및 효과음 볼륨
    [HideInInspector] public int sfxCnt; // 효과음 수

    public Sprite offSoundSprite;
    public Sprite onSoundSprite;
    [HideInInspector] public Image bgmImg;
    [HideInInspector] public Image sfxImg;
    // 싱글톤
    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            // 씬 전환 시 파괴 X
            DontDestroyOnLoad(instance);

            // 볼륨 초기화
            bgmVolume = 0.9f;
            sfxVolume = 0.6f;

            // (타입, 배경음) 맵핑
            Map();
        }
        else Destroy(gameObject);
    }
    
    // (타입, 배경음) 맵핑
    private void Map() { for(int i = 0; i < bgmList.Length; i++) mapBgm.Add((BgmType)i, bgmList[i]); }

    // 배경음
    public void BgmSoundPlay(BgmType bgmType)
    {
        // 음원 할당
        bgmSound.clip = mapBgm[bgmType];

        // 음원 반복
        bgmSound.loop = true;

        // 음원 볼륨
        bgmSound.volume = bgmVolume;

        // 음원 재생
        bgmSound.Play();
    }

    // 효과음
    public void SFXPlay(SoundType type)
    {
        PoolManager.instance.GetPool(PoolManager.instance.soundPool.queMap, type).GetComponent<AudioSource>().volume = sfxVolume;
        sfxCnt++;
    }

    // 배경음 볼륨 조절
    public void SetBgmVolume(float volume)
    {
        // 슬라이더 값에따라 볼륨 적용
        bgmSound.volume = volume;

        // 슬라이더 값을 변수에 저장해서 배경음악을 실행할때마다 볼륨을 지정
        bgmVolume = volume;

        bgmImg.sprite = bgmVolume == 0 ? offSoundSprite : onSoundSprite;
    }

    // 효과음 볼륨 조절
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        sfxImg.sprite = sfxVolume == 0 ? offSoundSprite : onSoundSprite;
    } 

}
