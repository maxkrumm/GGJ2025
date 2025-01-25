using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] Bubble _bubulePrefab;
    public GameObject meter;
    public int num;
   public int push_count;
   public float timeleft;
    public int bubblesize=0;
    const int Max=99;
    const int Min = 0;
    bool changeRotate;
    float rotate;
    float angle;
    Vector2 dir;



    void Meter()
    {
        if(0.1f>=meter.transform.eulerAngles.x)
        {
            changeRotate = true;
        }
        if(90<=meter.transform.eulerAngles.x)
        {
            changeRotate = false;
        }

        if(changeRotate)
        {
            rotate = 1;
        }
        else
        {
            rotate = -1;
        }

        meter.transform.Rotate(rotate,0,0);
        angle = meter.transform.eulerAngles.x;
        dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

    }

 
    // Update is called once per frame
    void Update()
    {
        Meter();
    
        if (Input.mouseScrollDelta.y > 0)
        {
            num--;
          
        }
        else if (Input.mouseScrollDelta.y<0)
        {
            num++;
           
        }
        BubbleType bubbleTypes= BubbleType.Dram;
        if (num > 0)
        {
             bubbleTypes = BubbleType.Dram;
           
        }
        if (num > 33)
        {
             bubbleTypes = BubbleType.Guitar;
        }
        if (num > 66)
        {
             bubbleTypes = BubbleType.Bass;
        }
        if (push_count > 1)
        {
            bubblesize = 1;
        }
        if (push_count >= 3)
        {
            bubblesize = 2;
        }
        if (push_count >= 5)
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
                var bubble = Instantiate(_bubulePrefab);
                bubble.Initialize(bubbleTypes, bubblesize, dir);
            }

        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            push_count = 0;
            bubblesize = 0;
        }
        Debug.Log(push_count);
        Debug.Log(num);
        Debug.Log(bubbleTypes);
        if (num <= Min)
        {
            num = Min;
        }
         if (num > Max)
            {
                num=Max;
            }
    }
    
    
}
