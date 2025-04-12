using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Color = UnityEngine.Color;

// �w�i��Bubble�̐��ɂ���ĕω������܂�
public class BackGroundManager : MonoBehaviour
{
    [SerializeField] private Sprite[] m_Sprites;

    [SerializeField] private SpriteRenderer[] renderers;
    private int currentSprite;

    public static BackGroundManager Instance;

    private List<Bubble> bubbles = new List<Bubble>();

    void Start()
    {
        currentSprite = 0;
        renderers[0].sprite = m_Sprites[0];
        renderers[0].sortingOrder = -1;
        renderers[1].sortingOrder = -2;
        var color = renderers[1].color;
        color.a = 0;
        renderers[1].color = color;

        GameManager
            .Instance
            .CurrentBubbleCollection()
            .ObserveAdd()
            .Subscribe((CollectionAddEvent<Bubble> r) => Add(r.Value))
            .AddTo(this);

        GameManager
           .Instance
           .CurrentBubbleCollection()
           .ObserveRemove()
           .Subscribe((CollectionRemoveEvent<Bubble> r) => Remove(r.Value))
           .AddTo(this);
    }


    public void Remove(Bubble bubble)
    {
        bubbles.Remove(bubble);
    }

    public void Add(Bubble bubble)
    {
        // ���̃^�C�v����ł��܂܂�Ă��Ȃ����
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

        // �J�ڐ�̉摜����둤�ɕ\��
        renderers[next].sprite = spriteW;
        renderers[next].color = Color.white;

        float elapsedTime = 0f;

        // �t�F�[�h�C��
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
