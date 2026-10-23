using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GeneratePractices
{
    static readonly string[] Names = {
        "SampleScene",
        "Scene 1",
        "Plinko",
        "Script Movimiento (Básico)",
        "clase 3 Actividad 1",
        "Nave",
        "clase 4 Actividad 1 - Movimiento con flechas",
        "clase 4 Actividad 2 - Cambiar las teclas",
        "clase 5 Ejercicio 1 - Movimiento directo con Transform",
        "clase 5 Ejercicio 2 - Impulsos y fuerzas con Rigidbody2D",
        "clase 5 Ejercicio 3 - Velocidad, gravedad y colisiones",
        "Ejercicio 1 - Configurar las acciones de entrada",
        "Ejercicio 2 - Mover un personaje con el New Input System",
        "Ejercicio 3",
        "Reto final - Migrar la nave de equilibrio"
    };

    static string[] Descriptions = {
        "Objetos básicos: cubo y triángulo.",
        "Gravedad y colisiones 2D.",
        "Obstáculos y objetos que caen.",
        "Movimiento básico por teclado.",
        "Movimiento y objetos interactivos.",
        "Nave triangular controlable.",
        "Flechas, salto por impulso y cambio de color con C.",
        "Movimiento alternativo con WASD.",
        "Movimiento directo con Transform.",
        "Rigidbody2D, fuerza e impulso con espacio.",
        "Velocidad, gravedad y colisiones.",
        "Acciones de entrada con controles de teclado.",
        "Movimiento con New Input System.",
        "Práctica de movimiento e interacción.",
        "Nave controlada mediante New Input System."
    };

    [MenuItem("Practicas/Generar las 15 escenas")]
    public static void Generate()
    {
        Directory.CreateDirectory("Assets/Scenes");
        for (int i = 0; i < Names.Length; i++) MakeScene(i);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("PRACTICAS_GENERADAS=15");
    }

    static void MakeScene(int index)
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        var cameraObject = new GameObject("Main Camera");
        var camera = cameraObject.AddComponent<Camera>();
        camera.orthographic = true;
        camera.orthographicSize = 5;
        camera.backgroundColor = new Color(.035f, .045f, .11f);
        cameraObject.tag = "MainCamera";
        cameraObject.transform.position = new Vector3(0, 0, -10);

        var light = new GameObject("Directional Light");
        light.transform.rotation = Quaternion.Euler(50, -30, 0);
        light.AddComponent<Light>().type = LightType.Directional;

        if (index == 2)
        {
            var spawner = new GameObject("Plinko Board");
            spawner.AddComponent<PlinkoSpawner>();
            MakeBlock("Floor", new Vector2(0, -4), new Vector2(9, .3f), false);
        }
        else
        {
            bool ship = index == 5 || index == 6 || index == 7 ||
                        index == 12 || index == 14;
            var player = new GameObject(ship ? "Nave" : "Player");
            player.transform.position = Vector3.zero;
            var sr = player.AddComponent<SpriteRenderer>();
            sr.sprite = ship ? PracticeAssets.Triangle() : PracticeAssets.Square();
            sr.color = ship ? new Color(.2f, .85f, 1f) : new Color(.7f, .45f, 1f);
            player.transform.localScale = ship ? Vector3.one * .9f : Vector3.one * .8f;

            bool physics = index == 1 || index == 4 || index == 9 ||
                           index == 10 || index == 13;
            if (physics)
            {
                var rb = player.AddComponent<Rigidbody2D>();
                rb.gravityScale = index == 10 || index == 13 ? 1f : 0f;
                if (ship) rb.gravityScale = 0;
                if (ship) rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                if (ship || index != 10) player.AddComponent<BoxCollider2D>();
                else player.AddComponent<CircleCollider2D>();
            }

            if (index == 3 || index == 5 || index == 6 || index == 7 ||
                index == 8 || index == 9 || index == 10 ||
                index == 11 || index == 12 || index == 13 || index == 14)
            {
                var controller = player.AddComponent<PracticeController>();
                controller.mode = index == 8 ? PracticeController.Mode.Transform :
                    index == 9 ? PracticeController.Mode.Impulse :
                    index == 10 ? PracticeController.Mode.Gravity :
                    index == 7 ? PracticeController.Mode.AlternateKeys :
                    index == 12 || index == 14 ? PracticeController.Mode.NewInput :
                    index == 9 ? PracticeController.Mode.Force :
                    PracticeController.Mode.Transform;
                controller.body = player.GetComponent<Rigidbody2D>();
                controller.spriteRenderer = sr;
            }

            MakeBlock("Ground", new Vector2(0, -3.5f), new Vector2(9, .35f), false);
            MakeBlock("Left Wall", new Vector2(-4.5f, 0), new Vector2(.3f, 7f), false);
            MakeBlock("Right Wall", new Vector2(4.5f, 0), new Vector2(.3f, 7f), false);

            if (index == 1 || index == 4 || index == 10 || index == 13)
            {
                var obstacle = MakeBlock("Obstacle", new Vector2(1.7f, -1f),
                    new Vector2(1f, .5f), false);
                obstacle.AddComponent<CollisionFeedback>().target = sr;
            }
        }

        var label = new GameObject("Practice Information");
        var text = label.AddComponent<TextMesh>();
        text.text = Names[index] + "\n" + Descriptions[index] +
            "\nFlechas/WASD: mover | Espacio: impulso | C: color";
        text.fontSize = 36;
        text.characterSize = .09f;
        text.anchor = TextAnchor.UpperCenter;
        text.alignment = TextAlignment.Center;
        label.transform.position = new Vector3(0, 4.2f, 0);

        string path = "Assets/Scenes/" + Names[index] + ".unity";
        EditorSceneManager.SaveScene(scene, path);
    }

    static GameObject MakeBlock(string name, Vector2 position, Vector2 size,
        bool dynamic)
    {
        var go = new GameObject(name);
        go.transform.position = position;
        go.transform.localScale = new Vector3(size.x, size.y, 1);
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = PracticeAssets.Square();
        sr.color = new Color(.25f, .55f, .85f);
        go.AddComponent<BoxCollider2D>();
        if (dynamic) go.AddComponent<Rigidbody2D>();
        return go;
    }
}
