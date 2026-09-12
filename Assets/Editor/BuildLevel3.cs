using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

// One-time level builder. Open Level_3.unity, then run
// Tools > EchoGhost > Build Level 3 Layout from the menu bar.
// Safe to re-run: it deletes everything it previously built (matched by
// name) before rebuilding, so re-running never duplicates or piles up objects.
public static class BuildLevel3
{
    const float TILE_W = 2.56f;
    const float TILE_H = 1.28f;

    // Exact names of objects this script creates or removes on each run.
    static readonly string[] GeneratedNames = {
        "Platform_A", "Platform_Pit", "Platform_B", "Platform_D",
        "EchoPortal_L3", "Pillar_L3", "Switch_L3", "Goal_L3"
    };
    const string SpikePrefix = "Spike_L3";

    // Leftover objects from the old Level_2 layout that this script replaces.
    static readonly string[] OldLevel2Names = {
        "Wall (1)", "EchoPortal", "Door", "DoorButton", "sign_1", "sign_2"
    };

    [MenuItem("Tools/EchoGhost/Build Level 3 Layout")]
    public static void Build()
    {
        if (SceneManager.GetActiveScene().name != "Level_3")
        {
            if (!EditorUtility.DisplayDialog("Wrong scene",
                "The active scene is not 'Level_3'. Open Assets/Scenes/Level_3.unity first, then run this again.\n\nRun anyway?",
                "Run anyway", "Cancel"))
                return;
        }

        CleanupPreviousRun();
        CleanupOldLevel2Layout();

        // ---- Platform A: start / checkpoint / echo portal ----
        CreateGround("Platform_A", centerX: 6f, topY: 0f, width: 16f);
        CreateEchoPortal(x: 10f, topY: 0f);

        // ---- Gap with a spike hazard pit below (cross it with Echo + double jump) ----
        CreateGround("Platform_Pit", centerX: 22f, topY: -10f, width: 14f);
        for (int i = 0; i < 5; i++)
        {
            float sx = 16f + i * 2.5f;
            CreateSpike(SpikePrefix + "_" + i, sx, -10f);
        }

        // ---- Platform B: switch + pillar ----
        CreateGround("Platform_B", centerX: 38f, topY: 0f, width: 20f);
        var pillar = CreatePillarDoor(x: 44f, topY: 0f);
        CreateSwitch(x: 34f, topY: 0f, targetDoor: pillar);

        // ---- Platform D: goal ----
        CreateGround("Platform_D", centerX: 64f, topY: 0f, width: 20f);
        CreateGoal(x: 70f, topY: 0f, targetScene: "Level_4");

        // ---- Move Player + Echo to the new start, update parallax reference point ----
        float startX = 0f;
        PlacePlayerAndEcho(startX, topY: 0f);
        RetargetParallax(startX);

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        Debug.Log("Level 3 layout built. Save the scene (Ctrl+S) and press Play to test.");
    }

    // ---------------- cleanup ----------------

    static void CleanupPreviousRun()
    {
        foreach (var n in GeneratedNames)
        {
            var go = GameObject.Find(n);
            if (go != null) Object.DestroyImmediate(go);
        }
        // Multiple spikes share a name prefix, so find all of them explicitly.
        foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None))
        {
            if (t != null && t.gameObject != null && t.gameObject.name.StartsWith(SpikePrefix))
                Object.DestroyImmediate(t.gameObject);
        }
    }

    static void CleanupOldLevel2Layout()
    {
        foreach (var n in OldLevel2Names)
        {
            var go = GameObject.Find(n);
            if (go != null) Object.DestroyImmediate(go);
        }

        // Old Level_2 ground tiles (and anything left over from a previous
        // run of this script, since our own platforms are also tagged Ground).
        var grounds = GameObject.FindGameObjectsWithTag("Ground");
        foreach (var g in grounds) Object.DestroyImmediate(g);
    }

    // ---------------- helpers ----------------

    static GameObject NewObject(string name, float x, float y, float scaleX, float scaleY)
    {
        var go = new GameObject(name);
        go.transform.position = new Vector3(x, y, 0f);
        go.transform.localScale = new Vector3(scaleX, scaleY, 1f);
        return go;
    }

    static Sprite LoadSprite(string path)
    {
        var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (sprite == null)
            Debug.LogWarning("BuildLevel3: could not load sprite at " + path);
        return sprite;
    }

    static GameObject CreateGround(string name, float centerX, float topY, float width, float heightTiles = 2f)
    {
        float scaleX = width / TILE_W;
        float scaleY = heightTiles;
        float worldHalfHeight = (TILE_H * scaleY) / 2f;
        var go = NewObject(name, centerX, topY - worldHalfHeight, scaleX, scaleY);
        go.tag = "Ground";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = LoadSprite("Assets/Tiles/normal_platform_v2.png");

        var col = go.AddComponent<BoxCollider2D>();
        if (sr.sprite != null) col.size = sr.sprite.bounds.size;

        return go;
    }

    static GameObject CreateEchoPortal(float x, float topY)
    {
        var sprite = LoadSprite("Assets/Tiles/portal.png");
        float scale = 2f;
        float halfHeight = (sprite != null ? sprite.bounds.size.y : 1.44f) * scale / 2f;
        var go = NewObject("EchoPortal_L3", x, topY + halfHeight, scale, scale);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = -1;

        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        if (sprite != null) col.size = sprite.bounds.size;

        go.AddComponent<EchoPortal>();
        return go;
    }

    static GameObject CreateSpike(string name, float x, float topY)
    {
        var sprite = LoadSprite("Assets/Tiles/spikes.png");
        float scale = 2f;
        float halfHeight = (sprite != null ? sprite.bounds.size.y : 0.32f) * scale / 2f;
        var go = NewObject(name, x, topY + halfHeight, scale, scale);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        if (sprite != null) col.size = sprite.bounds.size * 0.8f; // slightly forgiving hitbox

        go.AddComponent<Spike>();
        return go;
    }

    static Door CreatePillarDoor(float x, float topY)
    {
        var closed = LoadSprite("Assets/Tiles/door_vertical_closed.png");
        var open = LoadSprite("Assets/Tiles/door_vertical_open.png");
        float scale = 3f;
        float halfHeight = (closed != null ? closed.bounds.size.y : 1.6f) * scale / 2f;
        var go = NewObject("Pillar_L3", x, topY + halfHeight, scale, scale);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = closed;

        var col = go.AddComponent<BoxCollider2D>();
        if (closed != null) col.size = closed.bounds.size;

        var door = go.AddComponent<Door>();
        door.closedSprite = closed;
        door.openSprite = open;
        return door;
    }

    static void CreateSwitch(float x, float topY, Door targetDoor)
    {
        var idle = LoadSprite("Assets/Tiles/terminal_idle.png");
        var pressed = LoadSprite("Assets/Tiles/terminal_pressed.png");
        float scale = 1.5f;
        float halfHeight = (idle != null ? idle.bounds.size.y : 0.5f) * scale / 2f;
        var go = NewObject("Switch_L3", x, topY + halfHeight, scale, scale);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = idle;

        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        if (idle != null) col.size = idle.bounds.size;

        var btn = go.AddComponent<DoorButton>();
        btn.idleSprite = idle;
        btn.pressedSprite = pressed;
        btn.targetDoor = targetDoor;
    }

    static void CreateGoal(float x, float topY, string targetScene)
    {
        var sprite = LoadSprite("Assets/Tiles/portal.png");
        float scale = 2f;
        float halfHeight = (sprite != null ? sprite.bounds.size.y : 1.44f) * scale / 2f;
        var go = NewObject("Goal_L3", x, topY + halfHeight, scale, scale);

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = new Color(0.6f, 1f, 0.7f, 1f); // tint green so it reads as the goal, not another portal

        var col = go.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        if (sprite != null) col.size = sprite.bounds.size;

        var teleporter = go.AddComponent<DoorTeleporter>();
        teleporter.door = null; // ungated: this is the level exit
        teleporter.targetScene = targetScene;
    }

    static void PlacePlayerAndEcho(float startX, float topY)
    {
        var player = GameObject.Find("Player");
        var echo = GameObject.Find("Echo");
        var cam = GameObject.Find("Main Camera");

        Vector3 spawn = new Vector3(startX + 2f, topY + 1.5f, 0f);
        if (player != null) player.transform.position = spawn;
        if (echo != null) echo.transform.position = spawn + new Vector3(2f, 0f, 0f);
        if (cam != null) cam.transform.position = new Vector3(spawn.x, spawn.y, cam.transform.position.z);
    }

    static void RetargetParallax(float startX)
    {
        var layers = Object.FindObjectsByType<BackgroundParallax>(FindObjectsSortMode.None);
        foreach (var layer in layers)
        {
            layer.camStartPos = new Vector2(startX, layer.camStartPos.y);
        }
    }
}
