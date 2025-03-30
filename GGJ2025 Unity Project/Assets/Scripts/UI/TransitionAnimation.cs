using UnityEngine;
using DG;
using DG.Tweening;

public class TransitionAnimation : MonoBehaviour
{
    // ƒQ[ƒ€‰æ–Ê‚ÌUI
    [SerializeField] private CanvasGroup grInfo;

    // ƒƒjƒ…[‰æ–Ê‚ÌUI
    [SerializeField] private CanvasGroup grMenu;

    [SerializeField] private Transform traiangleA;
    [SerializeField] private Transform traiangleB;
    [SerializeField] private Transform traiangleC;

    [SerializeField] private Transform traiangleMask;

    private void Start()
    {
        ScreenTransition();
    }

    private void ScreenTransition()
    {
        //var seq = DOTween.Sequence()
        //    .Append(traiangleA.DOAnchorPosX(0f, 2f))
        //    .Join(traiangleB.DOLocalMoveX(0f, 2f).SetDelay(0.5f))
        //    .Join(traiangleC.DOLocalMoveX(0f, 2f).SetDelay(0.5f))
        //    .Join(traiangleMask.DOLocalMoveX(0f, 2f).SetDelay(0.5f))
        //    .AppendCallback(() =>
        //    {
        //        traiangleA.localPosition = new Vector3(-10f, 0f, 0f);
        //        traiangleB.localPosition = new Vector3(-10f, 0f, 0f);
        //        traiangleC.localPosition = new Vector3(-10f, 0f, 0f);
        //        traiangleMask.localPosition = new Vector3(-10f, 0f, 0f);
        //    });
    }
}
