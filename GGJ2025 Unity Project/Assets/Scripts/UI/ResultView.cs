using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField] private Button _retryButton;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private BackGroundManager _backGroundManager;

    private bool _isFinish;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _canvas = GetComponent<Canvas>();
         _canvas.enabled = false;

        _retryButton.OnClickAsObservable().Subscribe(_ => 
        {
            SceneManager.LoadSceneAsync("GameScene");
        }).AddTo(this);
    }

    private void Update()
    {
        if (_timeManager.Count <= 0)
        {
            ShowView();
        }

        if (!_isFinish) return;

        if (Input.GetMouseButtonDown(0) )
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
    public void ShowView()
    {
        _canvas.enabled = true;
        _isFinish = true;
    }
}
