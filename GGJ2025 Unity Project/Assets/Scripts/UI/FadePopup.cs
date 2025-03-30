using DG.Tweening;
using Ricimi;
using UnityEngine;

public class FadePopup : Popup
{

    private CanvasGroup _canvasGroup;
    public float duration = 1.0f; // フェード時間

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    public override void Open()
    {
        FadeIn();
    }

    public override void Close()
    {
        FadeOut();
    }


    public void FadeIn()
    {
        _canvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuad);
    }

    public void FadeOut()
    {
        _canvasGroup.DOFade(0f, duration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => Destroy(gameObject)); // フェードアウト後に削除
    }

}
