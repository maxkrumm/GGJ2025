using Assets.Scripts.Common;
using Cysharp.Threading.Tasks;
using Ricimi;
using System;
using System.Threading;
using UniRx;
using UnityEngine;


public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextDialog _dialogPrefab; // �e�L�X�g�_�C�A���O�{�b�N�X:�폜�\��

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

    // �`���[�g���A���̈�A�̎葱��
    private async UniTaskVoid TutorialFlowAsync(CancellationToken cancellationToken)
    {
        _dialog.enabled = true;
        _dialog.Text = "���̃Q�[���͂���ڂ�ʂ�������荇���������肵�ăC���^���N�e�B�u�ȉ��y�����Q�[���ł��B\n�����o������Ńv���C���邱�Ƃ������������܂��B";
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);
        _dialog.enabled = false;
        var bubble = await �V���{���̐����w��(cancellationToken);
        await �V���{���̔j��w��(bubble, cancellationToken);
        await �z�C�[���w��();
        // moveDialog
        //_dialog.Text = "�X�y�[�X�{�^���ł���ڂ�ʂ𔭎˂ł��܂��B";
        //await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);

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

    private async UniTask<Bubble> �V���{���̐����w��(CancellationToken cancellationToken)
    {
        var message = MessageBox.ShowText();
        message.Text = "�X�y�[�X�{�^���ł���ڂ�ʂ𔭎�";
        message.Open();

        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space), cancellationToken: cancellationToken);
        message.Close();

        var bubble = _bubbleGenerator.Generate(BubbleType.Amb);
        bubble._speed = 5.0f;
        Vector2 dir = new Vector2(Mathf.Cos(45), Mathf.Sin(45));
        bubble.Shoot(dir);
        return bubble;
    }

    private async UniTask �V���{���̔j��w��(Bubble bubble, CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: cancellationToken);
        bubble._speed = 0;

        var message = MessageBox.ShowText(RectTransformUtility.WorldToScreenPoint(
             Camera.main,
             bubble.transform.position + Vector3.up));
        message.Text = "�N���b�N�Ŕj��";
        message.Open();

        await bubble.OnDestroyedAsync;
        message.Close();
    }


    private async UniTask �z�C�[���w��()
    {
        var message = MessageBox.ShowText();
        message.Open();
        message.Text = "�z�C�[���ŃV���{���t�ύX";
        _bottleAnimator.Play("Tutorial_Bottle_RotationAnim");
        await UniTask.DelayFrame(400); // ��L�̉�]Animation����������܂őҋ@
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
