using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum BubbleType { �h����, �M�^�[, �x�[�X }

public class Bubble : MonoBehaviour
{
    [SerializeField] private Bubble prefab;
    [SerializeField] private BubbleSetting[] _settings;

    private AudioClip _moveSE;
    private AudioClip _breakSE;
    private AudioClip _blendSE;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;


    public Bubble _overlapBubble;

    private Rigidbody2D _rigidbody;
    public Vector2 Dir { get; set; }

    private int level = 1;
    private int size;
    public BubbleType Type { get; private set; }
    void Start()
    {

    }

    /// <summary>
    /// �V���{���ǂ������d�Ȃ��Ă��邩
    /// </summary>
    public bool IsOverlap => _overlapBubble != null;

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
        _spriteRenderer = GetComponent<SpriteRenderer>();

        var setting = _settings.FirstOrDefault(x => x.type == type);
        _spriteRenderer.color = setting.colors[level - 1];
        this.size = size;
        Dir = dir;

    }


    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = Dir * 5.0f;
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
        _audioSource.PlayOneShot(_blendSE);
        // Note:���ꂾ�Ɣj��Ɠ�����SE���~�܂邽�ߗv�C��
        Destroy(gameObject, 0.5f);
    }

    /// <summary>
    /// �V���{���ǂ���������
    /// </summary>
    public Bubble BlendBubbles()
    {

        var bubule = Instantiate(prefab);
        // _audioSource.PlayOneShot(_blendSE);
        bubule.level = level + 1;
        bubule.transform.position = transform.position;
        bubule.Initialize(Type, size, Dir);

        Destroy(this.gameObject, 0.1f);
        Destroy(_overlapBubble.gameObject, 0.1f);

        return bubule;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != null) return; // ���ɕʂ̂Əd�Ȃ��Ă���ꍇ�A�����͕s�v�̂���

        if (bubble.Type != Type || bubble.level != level) return;

        _overlapBubble = bubble;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != bubble) return;
        _overlapBubble = null;
    }

}
