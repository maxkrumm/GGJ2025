using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TimeManager _timeManager;
    [SerializeField] private BackGroundManager _backGroundManager;
    [SerializeField] private Image discImage;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject UICollider;

    private bool _isFinish;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _canvas = GetComponent<Canvas>();
         _canvas.enabled = false;
    }

    private void Update()
    {
        if (_timeManager.Count <= 0)
        {
            UI.SetActive(false);
            UICollider.SetActive(false);
            ShowView();
        }

        if (!_isFinish) return;

        discImage.rectTransform.Rotate(new Vector3(0, 0, 1) * speed);
    }
    public void ShowView()
    {
        _canvas.enabled = true;
        _isFinish = true;
    }

    public void RestartGame()
    {
        AkSoundEngine.PostEvent("Play_Home", gameObject);
        Debug.Log("Restart Game");
        SceneManager.LoadScene("TitleScene");
    }

}
