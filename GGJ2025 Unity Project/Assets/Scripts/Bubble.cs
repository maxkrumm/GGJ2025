using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum BubbleType { Dram, Guitar, Bass }

public class Bubble : MonoBehaviour
{
    [SerializeField] private BubbleSetting[] _settings;

    [SerializeField] private float _defaultSpeed = 1.0f;
    [SerializeField] private float _overlapedSpeed = 0.5f;

    [Header("サイズ係数")]
    [SerializeField] private float _sizeMultiplay = 1.0f;

    private AudioClip _moveSE;
    private AudioClip _breakSE;
    private AudioClip _blendSE;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;

    private float _speed = 1.0f;

    public Bubble _overlapBubble;

    private Rigidbody2D _rigidbody;
    public Vector2 Dir { get; set; }

    private int level = 1;
    private int size;

    /// <summary>
    /// 
    /// </summary>
    private float _stopTimer = 0.0f;

    public BubbleType Type { get; private set; }
    void Start()
    {

    }

    /// <summary>
    /// シャボンどおしが重なっているか
    /// </summary>
    public bool IsOverlap => _overlapBubble != null;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="type">シャボン玉種別</param>
    /// <param name="size">シャボン玉のサイズ3段階</param>
    public void Initialize(BubbleType type, int size, Vector2 dir)
    {
        transform.localScale = new Vector2(size, size) * _sizeMultiplay;
        _audioSource = this.AddComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        var setting = _settings.FirstOrDefault(x => x.type == type);
        _spriteRenderer.color = setting.colors[level - 1];
        this.size = size;
        Dir = dir;

        _speed = _defaultSpeed;
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
    /// Focusされている
    /// </summary>
    public void OnFocus(ref bool isFocus)
    {

    }

    /// <summary>
    /// バブル破壊メソッド
    /// </summary>
    public void BreakBubble()
    {
        //_audioSource.PlayOneShot(_blendSE);
        // Note:これだと破壊と同時にSEが止まるため要修正
        Destroy(gameObject);
    }
    [SerializeField] private Bubble prefab;
    /// <summary>
    /// シャボンどうしを合成
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

        if (_overlapBubble != null) return; // 既に別のと重なっている場合、合成は不要のため

        if (bubble.Type != Type || bubble.level != level||bubble.size!=size) return;

        _overlapBubble = bubble;

        // 重複時はクリックしやすいよう挙動をゆっくりに
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
