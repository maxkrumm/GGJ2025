using UnityEngine;

public enum BubbleType { �h����, �M�^�[, �x�[�X }

public class Bubble : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    /// <summary>
    /// �V���{���ǂ������d�Ȃ��Ă��邩
    /// </summary>
    public bool IsOverlap { get; private set; }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="type">�V���{���ʎ��</param>
    /// <param name="size">�V���{���ʂ̃T�C�Y3�i�K</param>
    public void Initialize(BubbleType type, int size)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Focus����Ă���
    /// </summary>
    public void OnFocus(ref bool isFocus)
    {

    }

    /// <summary>
    /// �o�u���j�󃁃\�b�h
    /// </summary>
    public void BreakBubble()
    {

    }

    /// <summary>
    /// �V���{���ǂ���������
    /// </summary>
    public void BlendBubbles()
    {

    }
}
