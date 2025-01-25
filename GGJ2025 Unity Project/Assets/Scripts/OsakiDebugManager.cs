using UnityEngine;

public class OsakiDebugManager : MonoBehaviour
{
    [SerializeField] private Bubble _bubblePrefab;
    public int size = 1;
    public Vector2 dir = Vector2.zero;

    void Start()
    {
        var bubble = Instantiate(_bubblePrefab);
        bubble.Initialize(BubbleType.ƒhƒ‰ƒ€, size, dir);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
