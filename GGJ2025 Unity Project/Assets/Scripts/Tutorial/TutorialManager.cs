using Assets.Scripts.Common;
using Cysharp.Threading.Tasks;
using Ricimi;
using System;
using System.Threading;
using UniRx;
using UnityEngine;


public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextDialog _dialogPrefab; // テキストダイアログボックス:削除予定

    [SerializeField] private BubbleGenerator _bubbleGenerator;
    [SerializeField] private PlayerBubbleClicker _playerBubbleClicker;

    [SerializeField] private Player _player;
    [SerializeField] private PopupOpener _bubbuleShootPopup;
    [SerializeField] private Animator _bottleAnimator;

    private TextDialog _dialog;
    private SceneTransitionController _transitionController;

    private void Start()
    {
        _transitionController = FindFirstObjectByType<SceneTransitionController>();
        _dialog = Instantiate(_dialogPrefab);
        TutorialFlowAsync(destroyCancellationToken).Forget();
    }

    // チュートリアルの一連の手続き
    private async UniTaskVoid TutorialFlowAsync(CancellationToken cancellationToken)
    {
        _dialog.enabled = true;
        _dialog.Text = "このゲームはしゃぼん玉を作ったり合成させたりしてインタラクティブな音楽を作るゲームです。\n音を出せる環境でプレイすることを強く推奨します。";
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);
        _dialog.enabled = false;
        var bubble = await シャボンの生成指示(cancellationToken);
        await シャボンの破壊指示(bubble, cancellationToken);
        await ホイール指示();
        // moveDialog
        //_dialog.Text = "スペースボタンでしゃぼん玉を発射できます。";
        //await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);

        //var message = MessageBox.ShowText();
        //message.Open();
        ////var popup = _bubbuleShootPopup.OpenPopup();

        //// 入力有効化
        //_player.enabled = true;
        //await _player.WaitForBubbleCreationAsync();
        //message.Close();
        ////popup.Close();

        ////popup = _bubbuleShootPopup.OpenPopup();
        ////popup.
        //await _player.WaitForBubbleCreationAsync();
        ////popup.Close();
        //message.Text = "Holdでシャボン玉成長";
        //message.Open();


        //await _player.WaitForBubbleCreationAsync()
        //    .ToUniTaskAsyncEnumerable()
        //    .FirstAsync(bubble => bubble.Size > 2);


        //popup.Close();
    }

    private async UniTask<Bubble> シャボンの生成指示(CancellationToken cancellationToken)
    {
        var message = MessageBox.ShowText();
        message.Text = "スペースボタンでしゃぼん玉を発射";
        message.Open();

        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: cancellationToken);
        message.Close();

        var bubble = _bubbleGenerator.Generate(BubbleType.Amb);
        bubble._speed = 5.0f;
        Vector2 dir = new Vector2(Mathf.Cos(45), Mathf.Sin(45));
        bubble.Shoot(dir);
        return bubble;
    }

    private async UniTask シャボンの破壊指示(Bubble bubble, CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
        bubble._speed = 0;

        var message = MessageBox.ShowText(RectTransformUtility.WorldToScreenPoint(
             Camera.main,
             bubble.transform.position + Vector3.up));
        message.Text = "クリックで破壊";
        message.Open();

        await bubble.OnDestroyedAsync;
        message.Close();
    }


    private async UniTask ホイール指示()
    {
        var message = MessageBox.ShowText();
        message.Open();
        message.Text = "ホイールでシャボン液変更";
        _bottleAnimator.Play("Tutorial_Bottle_RotationAnim");
        await UniTask.DelayFrame(400); // 上記の回転Animationが完了するまで待機
        _player.enabled = true;
        var cts = new CancellationTokenSource();

        await UniTask.WhenAny(
            _player.CurrentBubbleType.SkipLatestValueOnSubscribe().ToUniTask(useFirstValue: true, cts.Token),
            UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: cts.Token));

        cts.Cancel();
        message.Close();


    }
    public void OnClickTransitionButton(string name)
    {
        _transitionController.TransitionAsync(name, 1, default).Forget();
    }

}
