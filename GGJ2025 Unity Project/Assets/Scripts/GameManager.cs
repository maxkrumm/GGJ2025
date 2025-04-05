using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Collections.ObjectModel;

public class GameManager
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = new GameManager();
            return _instance;
        }
    }

    private ObservableCollection<Bubble> _currentBubule;

    private GameManager()
    {
        _currentBubule = new ObservableCollection<Bubble>();
    }

    public void AddBubble(Bubble bubble)
    {
        _currentBubule.Add(bubble);
    }

    public void RemoveBubble(Bubble bubble)
    {
        _currentBubule.Remove(bubble);
    }

    public ObservableCollection<Bubble> CurrentBubbleCollection() => _currentBubule;
}