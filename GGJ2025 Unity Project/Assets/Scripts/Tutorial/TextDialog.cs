using UnityEngine;
using UnityEngine.UI;

public class TextDialog : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Image _image;
    [SerializeField] private Canvas _canvas;
    public string Text { get => _text.text; set => _text.text = value; }

    private void OnEnable()
    {
        _canvas.enabled = true;
    }

    private void OnDisable()
    {
        _canvas.enabled = false;
    }
}
