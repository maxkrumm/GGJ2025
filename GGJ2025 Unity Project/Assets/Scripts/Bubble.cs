using UnityEngine;

public enum BubbleType { ドラム, ギター, ベース }

public class Bubble : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    public void Initialize(BubbleType type, int size)
    {

    }

    // Update is called once per frame
    void Update()
    {

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

    }

    /// <summary>
    /// シャボンどうしを合成
    /// </summary>
    public void BlendBubbles()
    {

    }
}
