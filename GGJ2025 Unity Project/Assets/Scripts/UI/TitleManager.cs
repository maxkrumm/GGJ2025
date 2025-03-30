using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum GameScene { TITLE,GAME,TUTORIAL}
public class TitleManager : MonoBehaviour
{
    private static string[] _sceneNames = new string[] { "TitleScene", "GameScene", "TutorialScene" };

    [SerializeField] private Image _discImage;
    [SerializeField] private CanvasGroup _discCanvas;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _tutorialButton;
    private SceneTransitionController _sceneTransitionController;

    private UniTaskCompletionSource<GameScene> _transitionCTS;
    private UniTask<GameScene> TransitionResult=>_transitionCTS.Task;


    async void Start()
    {
        _sceneTransitionController = FindFirstObjectByType<SceneTransitionController>();

        _transitionCTS = new UniTaskCompletionSource<GameScene>();

        _startGameButton.onClick.AddListener(() => _transitionCTS.TrySetResult(GameScene.GAME));
        _tutorialButton.onClick.AddListener(() => _transitionCTS.TrySetResult(GameScene.TUTORIAL));

        var nextScene = await TransitionResult;

        TransitionAsync(nextScene);
    }

    // Update is called once per frame
    void Update()
    {
        _discImage.rectTransform.Rotate(new Vector3(0, 0, 1) * speed);
    }

    [SerializeField] private float fadeCount = 1.0f;
    public async UniTaskVoid OnClicked(CancellationToken cancellationToken)
    {
        var timer = 0f;
        AkSoundEngine.PostEvent("Play_Play", gameObject);
        while (timer < fadeCount)
        {
            float a = Mathf.Lerp(1, 0, timer / fadeCount);

            _discCanvas.alpha = a;

            await UniTask.Yield();
            timer += Time.deltaTime;
        }
        await SceneManager.LoadSceneAsync("GameScene");
    }

    private void TransitionAsync(GameScene sceneName)
    {
        _sceneTransitionController.TransitionAsync(_sceneNames[(int)sceneName], 1, default);
    }
}
