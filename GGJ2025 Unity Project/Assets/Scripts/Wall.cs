using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class Wall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.TryGetComponent<Bubble>(out var bubble)) return;
        var normal = collision.contacts[0].normal;

        bubble.Dir = Vector2.Reflect(collision.relativeVelocity, normal).normalized;
    }
}
