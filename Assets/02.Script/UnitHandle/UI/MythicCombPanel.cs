using UnityEngine;

public class MythicCombPanel : MonoBehaviour
{
    [Header ("신화 조합식")] [SerializeField] private MythicUnit mythicUnit;

    private void OnEnable()
    {
        mythicUnit.SelectMythic("배트맨");
    }
}
