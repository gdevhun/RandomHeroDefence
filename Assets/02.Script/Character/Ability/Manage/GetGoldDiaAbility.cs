using UnityEngine;

public class GetGoldDiaAbility : MonoBehaviour
{
    [Header ("이펙트 이동 속도")] [SerializeField] private float moveSpeed;
    [Header ("이펙트 투명 속도")] [SerializeField] private float alphaSpeed;
    private SpriteRenderer rend;
    private Color originAlpha;
    private Color alpha;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
        originAlpha = rend.color;
    }

    private void OnEnable()
    {
        //if(rend == null) return;
        alpha = originAlpha;
    }

    private void Update()
    {
        // 이펙트 업 및 투명
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        rend.color = alpha;
    }
}
