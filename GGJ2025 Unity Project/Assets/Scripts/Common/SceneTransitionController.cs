using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    [SerializeField] private RectTransform traiangleA;
    [SerializeField] private RectTransform traiangleB;
    [SerializeField] private RectTransform traiangleC;

    [SerializeField] private RectTransform traiangleMask;
    [SerializeField] private Canvas _canvas;

    private static SceneTransitionController _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
    
        _canvas.enabled = false;

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
        _canvas.enabled = true;
        var a = SceneManager.LoadSceneAsync(sceneName);
        a.allowSceneActivation = false;


        var sequence = DOTween.Sequence()
            .Append(traiangleA.DOAnchorPosX(800f, 3f)) // TransformからRectTransformへ変更
            .Join(traiangleB.DOAnchorPosX(800f, 3f).SetDelay(0.5f)) // SetDelayの適用
            .Join(traiangleC.DOAnchorPosX(800f, 3f).SetDelay(0.5f)); // RectTransform用メソッド

        sequence.AppendCallback(async () =>
        {
            a.allowSceneActivation = true;
            await a;
            await traiangleMask.DOAnchorPosX(800f, 2f); // Maskも同様に変更
            _canvas.enabled = false;
            ////// アニメーション完了後の処理
            traiangleA.anchoredPosition = new Vector3(-4000, 0f, 0f);
            traiangleB.anchoredPosition = new Vector3(-4000, 0f, 0f);
            traiangleC.anchoredPosition = new Vector3(-4000, 0f, 0f);
            traiangleMask.anchoredPosition = new Vector3(-4000, 0f, 0f);
        });

        // アニメーション完了を待機
        await sequence.Play().ToUniTask(cancellationToken: cancellationToken);

       
    }
}
