using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] Bubble _bubulePrefab;
    public int num;
   public int push_count;
   public float timeleft;
    public int bubblesize;
    const int Max=99;
    const int Min = 0;
 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var bubble=Instantiate(_bubulePrefab);
    
        if (Input.mouseScrollDelta.y > 0)
        {
            if (num < Min)
            {
                num++;
            }
        }
        else if (Input.mouseScrollDelta.y<0) {
            if (num > Max)
            {
                num--;
            }
        }
        BubbleType bubbleTypes= BubbleType.ドラム;
        if (num > 0)
        {
             bubbleTypes = BubbleType.ドラム;
            string Type = bubbleTypes.ToString();
        }
        if (num > 33 || num < 66)
        {
             bubbleTypes = BubbleType.ギター;
        }
        if (num > 66)
        {
             bubbleTypes = BubbleType.ベース;
        }
        if (push_count < 1)
        {
            bubblesize = 1;
        }
        if (push_count <= 3)
        {
            bubblesize = 2;
        }
        if (push_count <= 5)
        {
            bubblesize = 3;
        }
        if (Input.GetKey(KeyCode.Space))
        {

            timeleft -= Time.deltaTime;
            if (timeleft <= 0)
            {
                timeleft = 0.75f;
                push_count++;
            }
            bubble.Initialize(bubbleTypes, bubblesize);

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            push_count = 0;
        }
        Debug.Log(push_count);
        Debug.Log(num);
    }
    
    
}
