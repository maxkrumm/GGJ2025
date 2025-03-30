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
            .Append(traiangleA.DOAnchorPosX(800f, 3f)) // Transform����RectTransform�֕ύX
            .Join(traiangleB.DOAnchorPosX(800f, 3f).SetDelay(0.5f)) // SetDelay�̓K�p
            .Join(traiangleC.DOAnchorPosX(800f, 3f).SetDelay(0.5f)); // RectTransform�p���\�b�h

        sequence.AppendCallback(async () =>
        {
            a.allowSceneActivation = true;
            await a;
            await traiangleMask.DOAnchorPosX(800f, 2f); // Mask�����l�ɕύX
            _canvas.enabled = false;
            ////// �A�j���[�V����������̏���
            traiangleA.anchoredPosition = new Vector3(-4000, 0f, 0f);
            traiangleB.anchoredPosition = new Vector3(-4000, 0f, 0f);
            traiangleC.anchoredPosition = new Vector3(-4000, 0f, 0f);
            traiangleMask.anchoredPosition = new Vector3(-4000, 0f, 0f);
        });

        // �A�j���[�V����������ҋ@
        await sequence.Play().ToUniTask(cancellationToken: cancellationToken);

       
    }
}
