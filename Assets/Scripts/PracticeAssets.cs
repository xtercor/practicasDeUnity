using UnityEngine;

public static class PracticeAssets
{
    static Sprite circle;
    static Sprite square;
    static Sprite triangle;

    public static Sprite Circle()
    {
        if (circle) return circle;
        circle = Sprite.Create(Texture(64, 0), new Rect(0, 0, 64, 64),
            new Vector2(.5f, .5f), 64);
        return circle;
    }

    public static Sprite Square()
    {
        if (square) return square;
        square = Sprite.Create(Texture(64, 1), new Rect(0, 0, 64, 64),
            new Vector2(.5f, .5f), 64);
        return square;
    }

    public static Sprite Triangle()
    {
        if (triangle) return triangle;
        triangle = Sprite.Create(Texture(64, 2), new Rect(0, 0, 64, 64),
            new Vector2(.5f, .5f), 64);
        return triangle;
    }

    static Texture2D Texture(int size, int shape)
    {
        var t = new Texture2D(size, size, TextureFormat.RGBA32, false);
        t.filterMode = FilterMode.Point;
        var pixels = new Color[size * size];
        for (int y = 0; y < size; y++)
        for (int x = 0; x < size; x++)
        {
            float nx = (x + .5f) / size * 2 - 1;
            float ny = (y + .5f) / size * 2 - 1;
            bool inside = shape == 0 ? nx * nx + ny * ny < .85f :
                shape == 1 ? true :
                ny > -.75f && ny < .8f && Mathf.Abs(nx) < (.8f - ny) * .72f;
            pixels[y * size + x] = inside ? Color.white : Color.clear;
        }
        t.SetPixels(pixels);
        t.Apply();
        return t;
    }
}
