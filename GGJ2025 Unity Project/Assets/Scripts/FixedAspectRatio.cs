using UnityEngine;
using UnityEngine.UI;

public class FixedAspectRatio : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    private Camera _camera;

    public GameObject blackBarTop; // UI Panel for the top black bar
    public GameObject blackBarBottom; // UI Panel for the bottom black bar

    private void Start()
    {
        _camera = GetComponent<Camera>();
        UpdateCameraViewport();
    }

    private void UpdateCameraViewport()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float scale = screenAspect / targetAspect;

        if (scale < 1f) // Tall screen (black bars top & bottom)
        {
            _camera.rect = new Rect(0f, (1f - scale) / 2f, 1f, scale);
            ShowBlackBars((1f - scale) / 2f * Screen.height); // Adjust the bars
        }
        else if (scale > 1f) // Wide screen (black bars left & right)
        {
            float scaleWidth = 1f / scale;
            _camera.rect = new Rect((1f - scaleWidth) / 2f, 0f, scaleWidth, 1f);
            ShowBlackBars(0); // No top/bottom bars needed
        }
        else
        {
            _camera.rect = new Rect(0, 0, 1, 1);
            ShowBlackBars(0); // Hide bars in perfect 16:9
        }
    }

    private void ShowBlackBars(float barSize)
    {
        if (blackBarTop && blackBarBottom)
        {
            RectTransform topRect = blackBarTop.GetComponent<RectTransform>();
            RectTransform bottomRect = blackBarBottom.GetComponent<RectTransform>();

            topRect.sizeDelta = new Vector2(Screen.width, barSize);
            bottomRect.sizeDelta = new Vector2(Screen.width, barSize);

            blackBarTop.SetActive(barSize > 0);
            blackBarBottom.SetActive(barSize > 0);
        }
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black); // Prevents flickering
    }

    private void Update()
    {
        if (Screen.width != _camera.pixelWidth || Screen.height != _camera.pixelHeight)
        {
            UpdateCameraViewport();
        }
    }
}
