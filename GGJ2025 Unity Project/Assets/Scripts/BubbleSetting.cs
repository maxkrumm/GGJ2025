using UnityEngine;

[CreateAssetMenu(fileName = "BubbleSetting", menuName = "Scriptable Objects/BubbleSetting")]
public class BubbleSetting : ScriptableObject
{
    public BubbleType type;

    public Color[] colors;
}
