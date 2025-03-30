using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Image discImage;
    [SerializeField] private CanvasGroup discCanvas;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Button _transitionButton;

    public GameObject slider;

    async void Start()
    {
        await _transitionButton.OnClickAsync(this.GetCancellationTokenOnDestroy());
        OnClicked(destroyCancellationToken).Forget();
    }
    // Update is called once per frame
    void Update()
    {
        discImage.rectTransform.Rotate(new Vector3(0, 0, 1) * speed);
    }

    [SerializeField] private float fadeCount = 1.0f;
    public async UniTaskVoid OnClicked(CancellationToken cancellationToken)
    {
        var timer = 0f;
        AkSoundEngine.PostEvent("Play_Play", gameObject);
        while (timer < fadeCount)
        {
            float a = Mathf.Lerp(1, 0, timer / fadeCount);

            discCanvas.alpha = a;

            await UniTask.Yield();
            timer += Time.deltaTime;
        }
        await SceneManager.LoadSceneAsync("GameScene");
    }
}
