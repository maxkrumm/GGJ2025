using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private Text _timeText;
    [SerializeField] private Canvas _spriteRenderer;

    void Update()
    {
        _timeText.text = timeManager.Count.ToString();
        if (timeManager.Count <= 0)
        {
            _spriteRenderer.gameObject.SetActive(false);
        }
    }
}
