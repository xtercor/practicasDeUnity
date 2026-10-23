using UnityEngine;

public class CollisionFeedback : MonoBehaviour
{
    public SpriteRenderer target;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (target) target.color = Random.ColorHSV();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (target) target.color = Random.ColorHSV();
    }
}
