using UnityEngine;

public class SoundDeActive : MonoBehaviour
{
    private AudioSource audioSource; // 오디오소스
    [HideInInspector] public SoundType type; // 사운드 타입

    private void Awake() { audioSource = GetComponent<AudioSource>(); }

    // 사운드 재생이 끝나면 풀에 자동 반환
    private void Update()
    {
        if(!audioSource.isPlaying)
        {
            SoundManager.instance.sfxCnt--;
            PoolManager.instance.ReturnPool(PoolManager.instance.soundPool.queMap, gameObject, type);
        }
    }
}
