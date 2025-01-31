using UnityEngine;

public class PlayerBubbleClicker : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���N���b�N�����o
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider == null) return;

            
            var bubble = hit.collider.GetComponent<Bubble>();
            if (bubble.IsOverlap)
                bubble.BlendBubbles();
            else
                bubble.BreakBubble();  
            
        }
    }
}
