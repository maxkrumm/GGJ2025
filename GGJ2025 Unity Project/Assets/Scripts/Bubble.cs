using Unity.VisualScripting;
using UnityEngine;

public enum BubbleType { ドラム, ギター, ベース }

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
    /// シャボンどおしが重なっているか
    /// </summary>
    public bool IsOverlap { get; private set; }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="type">シャボン玉種別</param>
    /// <param name="size">シャボン玉のサイズ3段階</param>
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
        _audioSource.PlayOneShot(_blendSE);
        // Note:これだと破壊と同時にSEが止まるため要修正
        Destroy(gameObject,0.5f);
    }

    /// <summary>
    /// シャボンどうしを合成
    /// </summary>
    public void BlendBubbles()
    {
        _audioSource.PlayOneShot(_blendSE);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;

        if (_overlapBubble != null) return; // 既に別のと重なっている場合、合成は不要のため

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
