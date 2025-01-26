using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField]private TimeManager timeManager;
    [SerializeField] private Text _timeText;
    [SerializeField]private Canvas _spriteRenderer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timeText.text = timeManager.Count.ToString();
        if (timeManager.Count <= 0)
        {
            _spriteRenderer.gameObject.SetActive(false);
        }
    }
}
