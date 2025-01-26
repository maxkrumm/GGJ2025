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
    private Animator _animator;

    public float _speed = 0.0f;

    public Bubble _overlapBubble;

    private Rigidbody2D _rigidbody;
    public Vector2 Dir { get; set; }

    public int level = 1;
    public int size = 1;

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
    public void OnDestroy()
    {
        BackGroundManager.Instance.Remove(this);
    }
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="type">�V���{���ʎ��</param>
    /// <param name="size">�V���{���ʂ̃T�C�Y3�i�K</param>
    public void Initialize(BubbleType type)
    {
        //BackGroundManager.Instance.Add(this);
        SetScale(1);
        Type = type;
        typeInt = (int)Type;

        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        var setting = _settings.FirstOrDefault(x => x.type == type);
        _spriteRenderer.color = setting.colors[level - 1];

               
        
    }

    public void SetScale(int size)
    {
        this.size = size;
        transform.localScale = new Vector2(size, size) * _sizeMultiplay;
    }

    public void Shoot(Vector2 dir)
    {
        Dir = dir;

        BubbleAudioPlayer bubbleAudioPlayer = gameObject.GetComponent<BubbleAudioPlayer>();
        bubbleAudioPlayer.QueueSound();
        bubbleAudioPlayer.PlaySound();

        Debug.Log("BUBBLE: " + Type + "  Size " + size + "  Level " + level);

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
        _animator.SetTrigger("Break");
        Destroy(gameObject, 0.5f);
    }
    [SerializeField] private Bubble prefab;
    /// <summary>
    /// �V���{���ǂ���������
    /// </summary>
    public Bubble BlendBubbles()
    {
        //_animator.SetTrigger("Break");

        var bubule = Instantiate(prefab);
        // _audioSource.PlayOneShot(_blendSE);
        bubule.level = level + 1;
        bubule.transform.position = transform.position;
        bubule.Initialize(Type);
        bubule.SetScale(size);
        bubule.Shoot(Dir);

        Destroy(this.gameObject, 0.1f);
        Destroy(_overlapBubble.gameObject, 0.1f);

        return bubule;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != null) return; // ���ɕʂ̂Əd�Ȃ��Ă���ꍇ�A�����͕s�v�̂���

        if (bubble.Type != Type || bubble.level != level || bubble.size != size) return;

        _overlapBubble = bubble;

        // �d�����̓N���b�N���₷���悤��������������
            _speed = _overlapedSpeed;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != bubble) return;
        _overlapBubble = null;

        _speed = _defaultSpeed;

    }


}
