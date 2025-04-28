using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{

    [SerializeField] private BubbleSetting[] _settings;
    [SerializeField] private Transform _spawnPoint;

    public Bubble Generate(BubbleType generateType)
    {
        Bubble bubble = Instantiate(_settings[(int)generateType].levelprefab[0], _spawnPoint.position, Quaternion.identity);
        bubble.Initialize(generateType);

        int size = 1;
        bubble.SetScale(size);
        return bubble;
    }
}
