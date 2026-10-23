using UnityEngine;

public class PlinkoSpawner : MonoBehaviour
{
    public int rows = 6;
    public float spacing = 1.2f;

    void Start()
    {
        for (int row = 0; row < rows; row++)
        for (int col = 0; col <= row + 1; col++)
        {
            var peg = new GameObject("Obstacle");
            peg.transform.position = new Vector3(
                (col - (row + 1) / 2f) * spacing, 3f - row * spacing, 0);
            var sr = peg.AddComponent<SpriteRenderer>();
            sr.sprite = PracticeAssets.Circle();
            sr.color = new Color(.25f, .8f, 1f);
            peg.transform.localScale = Vector3.one * .22f;
            peg.AddComponent<CircleCollider2D>();
        }

        for (int i = 0; i < 5; i++)
        {
            var ball = new GameObject("FallingBall");
            ball.transform.position = new Vector3((i - 2) * .7f, 5f + i, 0);
            var sr = ball.AddComponent<SpriteRenderer>();
            sr.sprite = PracticeAssets.Circle();
            sr.color = new Color(1f, .6f, .2f);
            ball.transform.localScale = Vector3.one * .35f;
            ball.AddComponent<CircleCollider2D>();
            var rb = ball.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1.5f;
        }
    }
}
