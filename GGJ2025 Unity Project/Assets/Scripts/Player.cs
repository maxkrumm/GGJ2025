using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bubble _bubulePrefab;
    public GameObject meter;
    private int limitcreate;
    public int num = 7;
    public int push_count;
    public float timeleft;
    public int bubblesize = 1;
    const int Max = 15;
    const int Min = 0;
    bool changeRotate;
    float rotate;
    float angle;
    Vector2 dir;
    Bubble bubblecs = new Bubble();
    public GameObject bottles;

    private List<Bubble> chargedBubbles = new List<Bubble>();
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isRunning; // Prevent multiple Init() loops
    private BubbleType _previousBubbleType; // Store last bubble type

    private void Start()
    {
        _previousBubbleType = BubbleType.Rythm; // Initialize with default
        StartInitLoop();
    }

    private void StartInitLoop()
    {
        if (_isRunning) return; // Prevent multiple loops

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
                if (!this.enabled || !gameObject.activeSelf || token.IsCancellationRequested) break;

                Bubble bubble = null;
                BubbleType bubbleTypes = BubbleType.Rythm;

                if (Input.mouseScrollDelta.y > 0) num--;
                else if (Input.mouseScrollDelta.y < 0) num++;

                num = Mathf.Clamp(num, Min, Max);

                if (num >= 0) 
                {
                    bubbleTypes = BubbleType.Amb;
                    bottles.transform.rotation = Quaternion.Euler(0, 0, -40);
                }
                if (num >= 5) 
                {
                    bubbleTypes = BubbleType.Rythm;
                    bottles.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                if (num >= 10) 
                {
                    bubbleTypes = BubbleType.Arp;
                    bottles.transform.rotation = Quaternion.Euler(0, 0, 40);
                }

                // ✅ Only trigger Play_Switch when the bubble type changes
                if (bubbleTypes != _previousBubbleType)
                {
                    AkSoundEngine.PostEvent("Play_Switch", gameObject);
                    _previousBubbleType = bubbleTypes; // Update previous state
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
                dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                bubble.Shoot(dir);
            }
        }
        finally
        {
            _isRunning = false; // Reset flag when exiting the loop
        }
    }

    private void OnDisable()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _isRunning = false; // Allow Init() to restart if needed
    }
}
