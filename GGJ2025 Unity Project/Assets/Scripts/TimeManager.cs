using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Unity.Content;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public int Count => count;
    [SerializeField] private int count;
    [SerializeField]private PlayerBubbleClicker bubbleClicker;
    [SerializeField]private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CountAsync(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid CountAsync(CancellationToken cancellationToken)
    {
        while (Count > 0&&!cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1),cancellationToken: cancellationToken);
            count--;
        }
        player.enabled = false;
        bubbleClicker.enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
