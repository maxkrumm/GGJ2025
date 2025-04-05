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

    private async UniTaskVoid TutorialFlowAsync(CancellationToken cancellationToken)
    {
        _dialog.enabled = true;
        _dialog.Text = "���̃Q�[���͂���ڂ�ʂ�������荇���������肵�ăC���^���N�e�B�u�ȉ��y�����Q�[���ł��B\n�����o������Ńv���C���邱�Ƃ������������܂��B";
        await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);

        // moveDialog
        //_dialog.Text = "�X�y�[�X�{�^���ł���ڂ�ʂ𔭎˂ł��܂��B";
        //await UniTask.WaitUntil(() => Input.GetMouseButtonDown(0), cancellationToken: cancellationToken);
        _dialog.enabled = false;

        var popup = _bubbuleShootPopup.OpenPopup();
        _player.enabled = true;
        await _player.WaitForBubbleCreationAsync();
        popup.Close();

        popup = _bubbuleShootPopup.OpenPopup();
        //popup.
        await _player.WaitForBubbleCreationAsync();
        popup.Close();

        await _player.WaitForBubbleCreationAsync()
            .ToUniTaskAsyncEnumerable()
            .FirstAsync(bubble => bubble.Size > 2);


        //popup.Close();
    }

    public void OnClickTransitionButton(string name)
    {
        new Subject<int>().Select().Subscribe(x =>)
        _transitionController.TransitionAsync(name, 1, default);
    }

}
