    using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField]private Sprite[] m_Sprites;
    
    [SerializeField] private SpriteRenderer[] renderers;
    private int currentSprite;

    public static BackGroundManager Instance;

    private List<Bubble> bubbles  = new List<Bubble>();
    
    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(FadeCoroutine(1));
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Remove(Bubble bubble)
    {
        bubbles.Remove(bubble);
    }
    public void Add(Bubble bubble) 
    {
        // そのタイプが一つでも含まれていなければ
        //if (!bubbles.Any(x => x.Type == bubble.Type))
        //{
        //    var sprite = m_Sprites[(int)bubble.Type];
        //    //renderers
        //    StartCoroutine(FadeCoroutine(sprite));
        //}
        //bubbles.Add(bubble);
    }


    [SerializeField]private float duration = 5f;
    private IEnumerator FadeCoroutine(Sprite spriteW)
    {
        var next = currentSprite == 1 ? 0 : 1;

        // 遷移先の画像を後ろ側に表示
        renderers[next].enabled = true;
        renderers[next].sprite = spriteW;


        float elapsedTime = 0f;

        // フェードイン
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            var color = renderers[currentSprite].color;
            color.a = alpha;
            renderers[currentSprite].color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderers[currentSprite].enabled = false;

        currentSprite = next;
    }
}
