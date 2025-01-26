using UnityEngine;

public class OsakiDebugManager : MonoBehaviour
{
    [SerializeField] private Bubble _bubblePrefab;
    public int size = 1;
    public Vector2 dir = Vector2.zero;

     public Bubble bubble1;
     public Bubble bubble2;

    void Start()
    {
        //bubble1 = Instantiate(_bubblePrefab);
        //bubble1.Initialize(BubbleType.Rythm, size, dir);

        //bubble2 = Instantiate(_bubblePrefab);
        //bubble2.Initialize(BubbleType.Rythm, size, dir);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            if (bubble1.IsOverlap)
            {
                bubble1.BlendBubbles();

            }
        }
    }
}
