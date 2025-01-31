using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bubble _bubulePrefab;
    public GameObject meter;
    public GameObject bottles; // Assign this in the inspector

    private int num = 7;
    private const int Max = 15;
    private const int Min = 0;
    private float previousRotation = 0f;
    private float currentRotation = 0f;
    public float duration = 0.1f; // Time for interpolation

    private bool isInterpolating = false; // Lock input when rotating

    private CancellationTokenSource _cancellationTokenSource;
    private bool _isRunning;
    private BubbleType _previousBubbleType;

    private void Start()
    {
        _previousBubbleType = BubbleType.Rythm;
        StartInitLoop();
    }

    private void StartInitLoop()
    {
        if (_isRunning) return;
        _cancellationTokenSource = new CancellationTokenSource();
        _isRunning = true;
        Init(_cancellationTokenSource.Token).Forget();
    }

    private async UniTask Init(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                if (!this.enabled || !gameObject.activeSelf) break;

                Bubble bubble = null;
                BubbleType bubbleTypes = BubbleType.Rythm;

                // Only allow num to increase when NOT interpolating
                if (!isInterpolating)
                {
                    if (Input.mouseScrollDelta.y > 0) num--;
                    else if (Input.mouseScrollDelta.y < 0) num++;
                }

                num = Mathf.Clamp(num, Min, Max);

                if (num >= 0)
                {
                    bubbleTypes = BubbleType.Amb;
                    currentRotation = -30f;
                }
                if (num >= 5)
                {
                    bubbleTypes = BubbleType.Rythm;
                    currentRotation = 0f;
                }
                if (num >= 10)
                {
                    bubbleTypes = BubbleType.Arp;
                    currentRotation = 30f;
                }

                // Only trigger rotation change when the target rotation is different
                if (!Mathf.Approximately(previousRotation, currentRotation))
                {
                    StartCoroutine(InterpolateToTarget(currentRotation, duration));
                }

                // Only trigger Play_Switch when the bubble type changes
                if (bubbleTypes != _previousBubbleType)
                {
                    AkSoundEngine.PostEvent("Play_Switch", gameObject);
                    _previousBubbleType = bubbleTypes;
                }

                if (!Input.GetKeyDown(KeyCode.Space))
                {
                    await UniTask.Yield(token);
                    continue;
                }

                float timer = 0f;
                bubble = Instantiate(_bubulePrefab, _spawnPoint.position, Quaternion.identity);
                bubble.Initialize(bubbleTypes);

                int size = 1;
                bubble.SetScale(size);

                while (Input.GetKey(KeyCode.Space) && !token.IsCancellationRequested)
                {
                    timer += Time.deltaTime;

                    if (timer >= 2)
                    {
                        bubble.SetScale(3);
                        break;
                    }
                    else if (timer >= 1)
                    {
                        bubble.SetScale(2);
                    }

                    await UniTask.Yield(token);
                }

                var angle = Random.Range(0, 360) % 90;
                bubble._speed = 5.0f;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                bubble.Shoot(dir);
            }
        }
        finally
        {
            _isRunning = false;
        }
    }

    IEnumerator InterpolateToTarget(float newTarget, float time)
    {
        float startValue = previousRotation;
        float elapsedTime = 0f;
        isInterpolating = true; // Prevent num from increasing

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            previousRotation = Mathf.Lerp(startValue, newTarget, elapsedTime / time);
            
            // Apply the interpolated rotation to the bottles object
            if (bottles != null)
            {
                bottles.transform.rotation = Quaternion.Euler(0, 0, previousRotation);
            }

            yield return null;
        }

        // Ensure final rotation is exactly correct
        previousRotation = newTarget;
        if (bottles != null)
        {
            bottles.transform.rotation = Quaternion.Euler(0, 0, previousRotation);
        }

        isInterpolating = false; // Allow num to increase again
    }

    private void OnDisable()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _isRunning = false;
    }
}
