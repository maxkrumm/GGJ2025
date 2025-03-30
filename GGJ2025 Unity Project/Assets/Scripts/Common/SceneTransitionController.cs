using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private Transform traiangleA;
    [SerializeField] private Transform traiangleB;
    [SerializeField] private Transform traiangleC;

    [SerializeField] private Transform traiangleMask;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void ScreenTransition()
    {
        var seq = DOTween.Sequence()
            .Append(traiangleA.DOLocalMoveX(0f, 2f))
            .Join(traiangleB.DOLocalMoveX(0f, 2f).SetDelay(0.5f))
            .Join(traiangleC.DOLocalMoveX(0f, 2f).SetDelay(0.5f))
            .Join(traiangleMask.DOLocalMoveX(0f, 2f).SetDelay(0.5f))
            .AppendCallback(() =>
            {
                traiangleA.localPosition = new Vector3(-10f, 0f, 0f);
                traiangleB.localPosition = new Vector3(-10f, 0f, 0f);
                traiangleC.localPosition = new Vector3(-10f, 0f, 0f);
                traiangleMask.localPosition = new Vector3(-10f, 0f, 0f);
            });
    }

    public async UniTask TransitionAsync(string sceneName, int type, CancellationToken cancellationToken)
    {
        var a = SceneManager.LoadSceneAsync(sceneName);
        a.allowSceneActivation = false;

        var sequence = DOTween.Sequence()
            .Append(traiangleA.DOLocalMoveX(0f, 2f))
            .Join(traiangleB.DOLocalMoveX(0f, 2f).SetDelay(0.5f))
            .Join(traiangleC.DOLocalMoveX(0f, 2f).SetDelay(0.5f));

        sequence.AppendCallback(async () =>
        {
            a.allowSceneActivation = true;
            await a;
            traiangleMask.DOLocalMoveX(0f, 2f);
        });

        // アニメーション完了を待機
        await sequence.Play().ToUniTask(cancellationToken: cancellationToken);

        ////// アニメーション完了後の処理
        //traiangleA.localPosition = new Vector3(-21, 0f, 0f);
        //traiangleB.localPosition = new Vector3(-21, 0f, 0f);
        //traiangleC.localPosition = new Vector3(-21, 0f, 0f);
        //traiangleMask.localPosition = new Vector3(-21, 0f, 0f);
    }
}
