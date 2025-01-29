using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using System.Linq;
using System.Drawing;
using Color = UnityEngine.Color;

public class BackGroundManager : MonoBehaviour
{
    [SerializeField] private Sprite[] m_Sprites;

    [SerializeField] private SpriteRenderer[] renderers;
    private int currentSprite;

    public static BackGroundManager Instance;

    private List<Bubble> bubbles = new List<Bubble>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentSprite = 0;
        renderers[0].sprite = m_Sprites[0];
        renderers[0].sortingOrder = -1;
        renderers[1].sortingOrder = -2;
        var color = renderers[1].color;
        color.a = 0;
        renderers[1].color = color;
    }

    public void Remove(Bubble bubble)
    {
        bubbles.Remove(bubble);
    }

    public void Add(Bubble bubble)
    {
        // そのタイプが一つでも含まれていなければ
        if (!bubbles.Any(x => x.Type == bubble.Type))
        {
            var sprite = m_Sprites[(int)bubble.Type];
            //renderers
            StartCoroutine(FadeCoroutine(sprite));
        }
        bubbles.Add(bubble);
    }


    [SerializeField] private float duration = 5f;
    private IEnumerator FadeCoroutine(Sprite spriteW)
    {
        var next = currentSprite == 1 ? 0 : 1;

        // 遷移先の画像を後ろ側に表示
        renderers[next].sprite = spriteW;
        renderers[next].color = Color.white;

        float elapsedTime = 0f;

        // フェードイン
        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            var color = renderers[currentSprite].color;
            color.a = alpha;
            renderers[currentSprite].color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderers[currentSprite].sortingOrder = -2;
        renderers[next].sortingOrder = -1;

        currentSprite = next;
    }
}
