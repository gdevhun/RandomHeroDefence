using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [Header ("텍스트 이동 속도")] [SerializeField] private float moveSpeed;
    [Header ("텍스트 투명 속도")] [SerializeField] private float alphaSpeed;
    [HideInInspector] public TextMeshProUGUI text;
    private Color originAlpha;
    private Color alpha;
    [Header ("텍스트 반환 시간")] [SerializeField] private float originWaitTime;
    private float waitTime;
    [Header ("플로팅 텍스트 타입")] [SerializeField] private FloatingTextType type;
    [HideInInspector] public Transform canvasTransform;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        originAlpha = text.color;
        canvasTransform = GameObject.Find("UI").transform;
    }

    private void OnEnable()
    {
        waitTime = originWaitTime;
        alpha = originAlpha;
    }

    private void Update()
    {
        // 텍스트가 위로올라가면서 투명
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        text.color = alpha;

        // 데미지 텍스트 반납
        if(waitTime <= 0)
        {
            PoolManager.instance.ReturnPool(PoolManager.instance.floatingTextPool.queMap, gameObject, type);
            EnemyBase.floatingDmgCnt--;
        }
        else waitTime -= Time.deltaTime;
    }
}
