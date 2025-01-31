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

    public bool isScaling = true;

    /// <summary>
    /// 
    /// </summary>
    private float _stopTimer = 0.0f;

    public int typeInt;
    public BubbleType Type { get; private set; }

    void Start()
    {

    }

    public bool IsOverlap => _overlapBubble != null;
    public void OnDestroy()
    {
        BackGroundManager.Instance.Remove(this);
    }


    public void Initialize(BubbleType type)
    {
        SetScale(1);
        Type = type;
        typeInt = (int)Type;

        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        var setting = _settings.FirstOrDefault(x => x.type == type);
        _spriteRenderer.color = setting.colors[level - 1];

        if(!IsOverlap)
        AkSoundEngine.PostEvent("Play_Grow", gameObject);  

        BackGroundManager.Instance.Add(this);

    }

    public void SetScale(int size)
    {
        isScaling = true;

        this.size = size;

        // Apply a smaller difference between sizes
        float adjustedSize = 1 + (size - 1) * 0.35f; // Scale size differences (1 -> 1, 2 -> 1.5, 3 -> 2)

        transform.localScale = new Vector2(adjustedSize, adjustedSize) * _sizeMultiplay;
    }


    public void Shoot(Vector2 dir, bool isBlend = false)
    {
        Dir = dir;

        BubbleAudioPlayer bubbleAudioPlayer = gameObject.GetComponent<BubbleAudioPlayer>();
        bubbleAudioPlayer.QueueSound();
        bubbleAudioPlayer.PlaySound();
        AkSoundEngine.PostEvent("Stop_Grow", gameObject);

        if(isBlend)
        AkSoundEngine.PostEvent("Play_Join", gameObject);
        else
        AkSoundEngine.PostEvent("Play_Spawn", gameObject);

        Debug.Log("BUBBLE: " + Type + "  Size " + size + "  Level " + level);

        isScaling = false;
    }

    // Update is called once per frame
    void Update()
    {


    }

    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = Dir * _speed;
    }

    public void OnFocus(ref bool isFocus)
    {

    }

    public void BreakBubble()
    {
        if(!isScaling)
        {
            AkSoundEngine.PostEvent("Play_Pop", gameObject);
            _animator.SetTrigger("Break");
            Destroy(gameObject, 0.5f);
        }
    }


    [SerializeField] private Bubble prefab;

    public Bubble BlendBubbles()
    {
        if(!_overlapBubble.isScaling)
        {           
            var bubule = Instantiate(prefab);
            bubule.level = Mathf.Clamp(level + 1, 1, 3);
            bubule.transform.position = transform.position;
            bubule.Initialize(Type);
            bubule.SetScale(size);
            bubule.Shoot(Dir, true);

            Destroy(this.gameObject, 0.1f);
            Destroy(_overlapBubble.gameObject, 0.1f);

            return bubule;
        }
        else
        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != null) return;

        if (bubble.Type != Type || bubble.level != level || bubble.size != size) return;

        _overlapBubble = bubble;

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
