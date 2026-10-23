using UnityEngine;
using UnityEngine.InputSystem;

public class PracticeController : MonoBehaviour
{
    public enum Mode { Transform, Velocity, Force, Impulse, Gravity, AlternateKeys, NewInput }
    public Mode mode;
    public float speed = 5f;
    public float force = 5f;
    public Rigidbody2D body;
    public SpriteRenderer spriteRenderer;
    private Vector2 move;
    private Keyboard keyboard;

    void Awake()
    {
        if (!body) body = GetComponent<Rigidbody2D>();
        if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
        keyboard = Keyboard.current;
    }

    void Update()
    {
        keyboard = Keyboard.current;
        if (keyboard == null) return;

        move = Vector2.zero;
        if (mode == Mode.AlternateKeys)
        {
            if (keyboard.aKey.isPressed) move.x -= 1;
            if (keyboard.dKey.isPressed) move.x += 1;
            if (keyboard.wKey.isPressed) move.y += 1;
            if (keyboard.sKey.isPressed) move.y -= 1;
        }
        else
        {
            if (keyboard.leftArrowKey.isPressed) move.x -= 1;
            if (keyboard.rightArrowKey.isPressed) move.x += 1;
            if (keyboard.downArrowKey.isPressed) move.y -= 1;
            if (keyboard.upArrowKey.isPressed) move.y += 1;
        }

        if (keyboard.spaceKey.wasPressedThisFrame && body)
        {
            if (mode == Mode.Impulse) body.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            else if (mode == Mode.Force) body.AddForce(Vector2.up * force, ForceMode2D.Force);
            else body.linearVelocity = new Vector2(body.linearVelocity.x, force);
        }

        if (keyboard.cKey.wasPressedThisFrame && spriteRenderer)
            spriteRenderer.color = Random.ColorHSV(0f, 1f, .65f, 1f, .8f, 1f);

        if (mode == Mode.Transform || mode == Mode.NewInput || mode == Mode.AlternateKeys)
            transform.position += (Vector3)(move.normalized * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (!body) return;
        if (mode == Mode.Velocity || mode == Mode.Gravity)
            body.linearVelocity = new Vector2(move.x * speed, body.linearVelocity.y);
    }
}
