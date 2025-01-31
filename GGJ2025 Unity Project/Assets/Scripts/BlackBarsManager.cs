using UnityEngine;

public class BlackBarController : MonoBehaviour
{
    [SerializeField] private RectTransform topBar;
    [SerializeField] private RectTransform bottomBar;

    private const float TargetAspect = 16f / 9f;

    void Start()
    {
        AdjustBars();
    }

    void Update()
    {
        AdjustBars(); // Continuously adjust in case the resolution changes
    }

    void AdjustBars()
    {
        float screenAspect = (float)Screen.width / Screen.height;

        // If the screen is exactly 16:9, remove the bars
        if (Mathf.Approximately(screenAspect, TargetAspect))
        {
            topBar.gameObject.SetActive(false);
            bottomBar.gameObject.SetActive(false);
            return;
        }

        // If the screen is taller than 16:9 (e.g., 4:3, 3:2), we add black bars
        if (screenAspect < TargetAspect)
        {
            topBar.gameObject.SetActive(true);
            bottomBar.gameObject.SetActive(true);

            // Calculate black bar height: how much extra height we need to cut off
            float heightDifference = ((TargetAspect / screenAspect) - 1f) * 0.5f * Screen.height;

            // Ensure the height is always positive
            heightDifference = Mathf.Abs(heightDifference);

            // Set the top and bottom bar sizes dynamically
            topBar.sizeDelta = new Vector2(Screen.width, heightDifference);
            bottomBar.sizeDelta = new Vector2(Screen.width, heightDifference);
        }
        else
        {
            // If the screen is wider than 16:9 (e.g., ultrawide 21:9), we should not show black bars
            topBar.gameObject.SetActive(false);
            bottomBar.gameObject.SetActive(false);
        }
    }
}
