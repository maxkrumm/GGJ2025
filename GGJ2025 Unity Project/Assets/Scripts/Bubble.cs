using Unity.VisualScripting;
using UnityEngine;

public enum BubbleType { �h����, �M�^�[, �x�[�X }

public class Bubble : MonoBehaviour
{
    private AudioClip _moveSE;
    private AudioClip _breakSE;
    private AudioClip _blendSE;
    private AudioSource _audioSource;

    private Bubble _overlapBubble;

    private Rigidbody2D _rigidbody;
    public Vector2 Dir { get;set;}

    private int level = 1;
    private int size;
    public BubbleType Type { get; private set; }
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
    public void Initialize(BubbleType type, int size, Vector2 dir)
    {
        transform.localScale = new Vector2(size, size);
        _audioSource = this.AddComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        var power = 5.0f;
        _rigidbody.AddForce(dir*power,ForceMode2D.Impulse);
        Dir = dir;  
    }

    // Update is called once per frame
    void Update()
    {

    }

  /* private void FixedUpdate()
    {
        _rigidbody.linearVelocity = Dir * 5.0f;
    }*/

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
        _audioSource.PlayOneShot(_blendSE);
        // Note:���ꂾ�Ɣj��Ɠ�����SE���~�܂邽�ߗv�C��
        Destroy(gameObject,0.5f);
    }

    /// <summary>
    /// �V���{���ǂ���������
    /// </summary>
    public void BlendBubbles()
    {
        _audioSource.PlayOneShot(_blendSE);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != null) return; // ���ɕʂ̂Əd�Ȃ��Ă���ꍇ�A�����͕s�v�̂���

        if (bubble.Type != Type || bubble.level != level) return;

        _overlapBubble = bubble;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != bubble) return;
        _overlapBubble = null;
    }
}
