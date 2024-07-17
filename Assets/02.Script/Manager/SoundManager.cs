using System.Collections.Generic;
using UnityEngine;

// 재생할 배경음 타입 => 키로 사용
public enum BgmType { Main }

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSound; // 배경음 오디오
	[SerializeField] private AudioClip[] bgmList; // 배경음 리스트
	private Dictionary<BgmType, AudioClip> mapBgm = new Dictionary<BgmType, AudioClip>(); // (타입, 배경음) 맵핑
	[HideInInspector] public float bgmVolume, sfxVolume; // 배경음 볼륨 및 효과음 볼륨

    // 싱글톤
    public static SoundManager instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

            // 볼륨 초기화
            bgmVolume = 0.1f;
            sfxVolume = 1f;

            // (타입, 배경음) 맵핑
            Map();

            // 초기 BGM 테스트
            BgmSoundPlay(BgmType.Main);
        }
        else Destroy(gameObject);
    }

    // 효과음 테스트
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0)) SFXPlay(SoundType.Spawn);
    }
    
    // (타입, 배경음) 맵핑
    private void Map()
    {
        for(int i = 0; i < bgmList.Length; i++) mapBgm.Add((BgmType)i, bgmList[i]);
    }

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
        PoolManager.instance.GetPool(PoolManager.instance.queSoundMap, type).GetComponent<AudioSource>().volume = sfxVolume;
    }

    // 배경음 볼륨 조절
    public void SetBgmVolume(float volume)
    {
        // 슬라이더 값에따라 볼륨 적용
        bgmSound.volume = volume;

        // 슬라이더 값을 변수에 저장해서 배경음악을 실행할때마다 볼륨을 지정
        bgmVolume = volume;
    }

    // 효과음 볼륨 조절
    public void SetSfxVolume(float volume)
    {
        // 슬라이더 값을 변수에 저장해서 효과음을 실행할때마다 볼륨을 지정
        sfxVolume = volume;
    }
}
