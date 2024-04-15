using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //变透明
    public void FadeOut()
    {
        Color targetColor = new Color(1, 1, 1, Settings.targetAlpha);
        spriteRenderer.DOColor(targetColor, Settings.itemDuration);
    }
    //变回原样
    public void FadeIn()
    {
        Color targetColor = new Color(1, 1, 1,1);
        spriteRenderer.DOColor(targetColor, Settings.itemDuration);
    }
}
