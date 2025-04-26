using Assets.Scripts.Common;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Ricimi;
using System;
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

    [SerializeField] private Animator _bottleAnimator;
    private void Start()
    {
        _transitionController = FindFirstObjectByType<SceneTransitionController>();
        _dialog = Instantiate(_dialogPrefab);
        TutorialFlowAsync(destroyCancellationToken).Forget();
    }

    // �`���[�g���A���̈�A�̎葱��
    private async UniTaskVoid TutorialFlowAsync(CancellationToken cancellationToken)
    {
        _dialog.enabled = true;
        _dialog.Text = "���̃Q�[���͂���ڂ�ʂ�������荇���������肵�ăC���^���N�e�B�u�ȉ��y�����Q�[���ł��B\n�����o������Ńv���C���邱�Ƃ������������܂��B";
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);

        await �z�C�[���w��();
        // moveDialog
        //_dialog.Text = "�X�y�[�X�{�^���ł���ڂ�ʂ𔭎˂ł��܂��B";
        //await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);
        _dialog.enabled = false;

        //var message = MessageBox.ShowText();
        //message.Open();
        ////var popup = _bubbuleShootPopup.OpenPopup();

        //// ���͗L����
        //_player.enabled = true;
        //await _player.WaitForBubbleCreationAsync();
        //message.Close();
        ////popup.Close();

        ////popup = _bubbuleShootPopup.OpenPopup();
        ////popup.
        //await _player.WaitForBubbleCreationAsync();
        ////popup.Close();
        //message.Text = "Hold�ŃV���{���ʐ���";
        //message.Open();


        //await _player.WaitForBubbleCreationAsync()
        //    .ToUniTaskAsyncEnumerable()
        //    .FirstAsync(bubble => bubble.Size > 2);


        //popup.Close();
    }

    private async UniTask �z�C�[���w��()
    {
        var message = MessageBox.ShowText();
        message.Open();
        message.Text = "�z�C�[���ŃV���{���t�ύX";
        _bottleAnimator.Play("Tutorial_Bottle_RotationAnim");
        await UniTask.DelayFrame(200);
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
