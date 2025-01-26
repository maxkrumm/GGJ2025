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

    private List<Bubble> bubbles  = new List<Bubble>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //StartCoroutine(FadeCoroutine(1));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Add(Bubble bubble) 
    {
        // そのタイプが一つでも含まれていなければ
        if (!bubbles.Any(x => x.Type == bubble.Type))
        {
            var sprite = m_Sprites[(int)bubble.Type];
            //renderers
            //StartCoroutine(FadeCoroutine());
        }
        bubbles.Add(bubble);
    }


    [SerializeField]private float duration = 5f;
    private IEnumerator FadeCoroutine(Sprite spriteW)
    {
        var next = currentSprite == 1 ? 0 : 1;

        // 遷移先の画像を後ろ側に表示
        renderers[next].enabled = true;

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
