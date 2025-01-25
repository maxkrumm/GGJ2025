using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum BubbleType { Rythm, Amb, Arp }

public class Bubble : MonoBehaviour
{
    [SerializeField] private BubbleSetting[] _settings;

    [SerializeField] private float _defaultSpeed = 1.0f;
    [SerializeField] private float _overlapedSpeed = 0.5f;

    [Header("�T�C�Y�W��")]
    [SerializeField] private float _sizeMultiplay = 1.0f;


    [Header("true�Ȃ�d�Ȃ����ꍇ�ɃX�s�[�h����������,false�̏ꍇ��莞�Ԏ~�܂�")]
    [SerializeField] private bool IsOverlapAction = false;

    private SpriteRenderer _spriteRenderer;

    private float _speed = 1.0f;

    public Bubble _overlapBubble;

    private Rigidbody2D _rigidbody;
    public Vector2 Dir { get; set; }

    [HideInInspector] public int level = 1;
    [HideInInspector] public int size = 1;

    /// <summary>
    /// 
    /// </summary>
    private float _stopTimer = 0.0f;

    public int typeInt;
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

        transform.localScale = new Vector2(size, size) * _sizeMultiplay;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        var setting = _settings.FirstOrDefault(x => x.type == type);
        _spriteRenderer.color = setting.colors[level - 1];
        this.size = size;
        Dir = dir;
        Type = type;
        Debug.Log("BUBBLE: " + Type + "  Size " + size + "  Level " + level);
        _speed = _defaultSpeed;
        typeInt = (int)Type;

        BubbleAudioPlayer bubbleAudioPlayer = gameObject.GetComponent<BubbleAudioPlayer>();
        bubbleAudioPlayer.QueueSound();
    }


    // Update is called once per frame
    void Update()
    {
        

    }

    private void FixedUpdate()
    {
       _rigidbody.linearVelocity = Dir * _speed;
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

        Destroy(gameObject);
    }
    [SerializeField] private Bubble prefab;
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

        if (bubble.Type != Type || bubble.level != level||bubble.size!=size) return;

        _overlapBubble = bubble;

        // �d�����̓N���b�N���₷���悤��������������
        if (IsOverlapAction)
            _speed = _overlapedSpeed;
        else
            _stopTimer = 2f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != bubble) return;
        _overlapBubble = null;

        _speed = _defaultSpeed;

    }

}
