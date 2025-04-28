using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using System;

public class Player : MonoBehaviour
{
    private const int Max = 2;
    private const int Min = 0;
    private BubbleType[] bubbleTypes = new BubbleType[3] { BubbleType.Arp, BubbleType.Rythm, BubbleType.Amb };
    private float[] bubbleTypeRots = new float[3] { 30, 0, -30 };

    [SerializeField] private BubbleGenerator _bubbleGenerator;
    public GameObject bottles; // Assign this in the inspector

    private int currentBubbleID = 1;
    private float previousRotation = 0f;
    private float currentRotation = 0f;
    public float duration = 0.1f; // Time for interpolation

    private bool isInterpolating = false; // Lock input when rotating
    private Subject<Bubble> _onBubbleCreated = new Subject<Bubble>();
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isRunning;
    private BubbleType _previousBubbleType;


    public IReadOnlyReactiveProperty<BubbleType> CurrentBubbleType => _bubbleRx;
    private ReactiveProperty<BubbleType> _bubbleRx = new ReactiveProperty<BubbleType>(BubbleType.Rythm);

    private void Start()
    {
        _bubbleRx.AddTo(this);
        _onBubbleCreated.AddTo(this);
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

    private void Update()
    {
        // Only allow num to increase when NOT interpolating

        if (isInterpolating) return;

        if (Input.mouseScrollDelta.y > 0) currentBubbleID++;
        else if (Input.mouseScrollDelta.y < 0) currentBubbleID--;

        currentBubbleID = Mathf.Clamp(currentBubbleID, Min, Max);

        _bubbleRx.Value = bubbleTypes[currentBubbleID];
        currentRotation = bubbleTypeRots[currentBubbleID];

        // Only trigger rotation change when the target rotation is different
        if (!Mathf.Approximately(previousRotation, currentRotation))
        {
            StartCoroutine(InterpolateToTarget(currentRotation, duration));
        }
    }

    private async UniTask Init(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                if (!this.enabled || !gameObject.activeSelf) break;

                Bubble bubble = null;



                // Only trigger Play_Switch when the bubble type changes
                if (_bubbleRx.Value != _previousBubbleType)
                {
                    AkSoundEngine.PostEvent("Play_Switch", gameObject);
                    _previousBubbleType = _bubbleRx.Value;
                }

                if (!Input.GetKeyDown(KeyCode.Space))
                {
                    await UniTask.Yield(token);
                    continue;
                }

                float timer = 0f;
                bubble = _bubbleGenerator.Generate(_bubbleRx.Value);


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

                var angle = UnityEngine.Random.Range(0, 360) % 90;
                bubble._speed = 5.0f;
                Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                bubble.Shoot(dir);

                _onBubbleCreated.OnNext(bubble);
            }
        }
        finally
        {
            _isRunning = false;
        }
    }

    public IObservable<Bubble> OnBubbleCreatedAsObservable()
    {
        return _onBubbleCreated.AsObservable();
    }

    public async UniTask<Bubble> WaitForBubbleCreationAsync()
    {
        return await OnBubbleCreatedAsObservable().First();
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
