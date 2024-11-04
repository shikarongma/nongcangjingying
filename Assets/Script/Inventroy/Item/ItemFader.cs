using DG.Tweening;
using UnityEngine;

//���ﾭ���Ƿ��͸��
[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //��͸��
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
        spriteRenderer.DOColor(targetColor, Settings.itemDuration);
    }
    //���ԭ��
    public void FadeIn()
    {
        Color targetColor = new Color(1, 1, 1,1);
        spriteRenderer.DOColor(targetColor, Settings.itemDuration);
    }
}
