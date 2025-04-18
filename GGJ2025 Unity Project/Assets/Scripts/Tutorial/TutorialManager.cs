using Assets.Scripts.Common;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Ricimi;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextDialog _dialogPrefab;
    //[SerializeField]
    //private
    [SerializeField] private Player _player;
    [SerializeField] private PopupOpener _bubbuleShootPopup;
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

        // moveDialog
        //_dialog.Text = "スペースボタンでしゃぼん玉を発射できます。";
        //await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);
        _dialog.enabled = false;

        var message = MessageBox.ShowText();
        message.Open();
        //var popup = _bubbuleShootPopup.OpenPopup();

        // 入力有効化
        _player.enabled = true;
        await _player.WaitForBubbleCreationAsync();
        message.Close();
        //popup.Close();

        //popup = _bubbuleShootPopup.OpenPopup();
        //popup.
        await _player.WaitForBubbleCreationAsync();
        //popup.Close();
        message.Text = "Holdでシャボン玉成長";
        message.Open();


        await _player.WaitForBubbleCreationAsync()
            .ToUniTaskAsyncEnumerable()
            .FirstAsync(bubble => bubble.Size > 2);


        //popup.Close();
    }

    public void OnClickTransitionButton(string name)
    {
        _transitionController.TransitionAsync(name, 1, default).Forget();
    }

}
