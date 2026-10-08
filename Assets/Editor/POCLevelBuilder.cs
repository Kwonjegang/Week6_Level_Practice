using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Builds a readable, non-interactive Unity graybox for the approved Week 6 level layout.
/// Geometry is authored along +X; the camera shows the full route from a high rear angle.
/// </summary>
public static class POCLevelBuilder
{
    private const string AssetRoot = "Assets/POC_Blockout";
    private const string MaterialsPath = AssetRoot + "/Materials";
    private const string ScenePath = "Assets/Scenes/POC_Level_Blockout.unity";

    private static Transform root;
    private static Camera overviewCamera;
    private static Font labelFont;

    private static Material concrete;
    private static Material floor;
    private static Material wall;
    private static Material dark;
    private static Material blue;
    private static Material blueLight;
    private static Material purple;
    private static Material yellow;
    private static Material yellowLight;
    private static Material red;
    private static Material rust;
    private static Material green;
    private static Material greenLight;
    private static Material steel;
    private static Material white;

    [MenuItem("Tools/POC/Build Week 6 Level Graybox")]
    public static void BuildAndSave()
    {
        EnsureFolder("Assets", "POC_Blockout");
        EnsureFolder(AssetRoot, "Materials");

        concrete = MakeMaterial("Concrete", new Color(0.54f, 0.55f, 0.54f));
        floor = MakeMaterial("Walkable Floor", new Color(0.72f, 0.72f, 0.69f));
        wall = MakeMaterial("Fixed Wall", new Color(0.17f, 0.19f, 0.20f));
        dark = MakeMaterial("Abyss", new Color(0.075f, 0.09f, 0.10f));
        blue = MakeMaterial("Ability Blue", new Color(0.12f, 0.48f, 0.83f));
        blueLight = MakeMaterial("Child Route Purple", new Color(0.50f, 0.31f, 0.70f));
        purple = MakeMaterial("Child Route Light", new Color(0.72f, 0.57f, 0.87f));
        yellow = MakeMaterial("Barrier Yellow", new Color(0.95f, 0.66f, 0.04f));
        yellowLight = MakeMaterial("Toxic Barrier Field", new Color(0.96f, 0.83f, 0.34f));
        red = MakeMaterial("Breakable Red", new Color(0.79f, 0.23f, 0.20f));
        rust = MakeMaterial("Corroded Support", new Color(0.42f, 0.27f, 0.17f));
        green = MakeMaterial("Toxic Green", new Color(0.22f, 0.54f, 0.35f));
        greenLight = MakeMaterial("Toxic Rim", new Color(0.48f, 0.73f, 0.55f));
        steel = MakeMaterial("Damaged Steel", new Color(0.39f, 0.43f, 0.45f));
        white = MakeMaterial("Label White", new Color(0.95f, 0.95f, 0.91f));

        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        GameObject levelRoot = new GameObject("POC_LEVEL_ROOT - 00 to 09");
        root = levelRoot.transform;
        labelFont = LoadLabelFont();
        overviewCamera = CreateOverviewCamera();

        CreateLighting();
        Build00();
        Build01();
        Build02And03();
        Build04();
        Build05();
        Build06And08();
        Build07And09();
        BuildWorldBase();
        AddNorthAndScaleLegend();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, ScenePath);
        AddSceneToBuildSettings(ScenePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("POC graybox saved to " + ScenePath + ". This scene is spatial layout only; runtime player abilities are not implemented.");
    }

    private static void Build00()
    {
        BuildRoomShell("00 START - quiet intro", 0f, 8f, -4f, 4f, 2.4f, 2.4f, false);
        Box("00 floor before small hop", new Vector3(1.9f, -0.25f, 0f), new Vector3(3.8f, 0.5f, 8f), floor);
        Box("00 floor after small hop", new Vector3(6.2f, -0.25f, 0f), new Vector3(3.6f, 0.5f, 8f), floor);
        Box("00 small movement gap pit", new Vector3(4.1f, -1.2f, 0f), new Vector3(0.6f, 1.6f, 8f), dark, false);
        Box("00 rubble left", new Vector3(1.2f, 0.3f, 2.4f), new Vector3(1.2f, 0.6f, 1.3f), concrete);
        Box("00 collapsed beam", new Vector3(6.2f, 0.6f, -2.5f), new Vector3(2.1f, 0.45f, 0.45f), rust).transform.rotation = Quaternion.Euler(0f, 0f, 11f);
        Marker("Girl - dormant intro", new Vector3(1.4f, 0.55f, -1.0f), 0.45f, blueLight);
        RobotPlaceholder("Robot - awakening intro", new Vector3(2.7f, 1.0f, 0.3f));
        Box("00 exit door frame left", new Vector3(7.94f, 1.45f, -1.55f), new Vector3(0.28f, 2.9f, 1.2f), steel);
        Box("00 exit door frame right", new Vector3(7.94f, 1.45f, 1.55f), new Vector3(0.28f, 2.9f, 1.2f), steel);
        Label("00  도입 · 조용한 시작", new Vector3(3.7f, 3.5f, 0f), white, 0.25f);
        Label("활성화 → 어부바 → 백뷰", new Vector3(3.9f, 2.8f, 0f), blue, 0.18f);
        Label("걷기 / 카메라 회전 / 짧은 점프", new Vector3(3.7f, 2.55f, 2.7f), blue, 0.16f);
        Label("작은 틈 · 낙하 없는 조작 연습", new Vector3(4.0f, 1.2f, -0.15f), white, 0.15f);
    }

    private static void Build01()
    {
        BuildRoomShell("01 BLUE - jump tutorial", 8f, 20f, -5f, 5f, 2.4f, 2.4f, false);
        Box("01 west landing", new Vector3(10.35f, -0.25f, 0f), new Vector3(4.7f, 0.5f, 10f), floor);
        Box("01 east landing", new Vector3(17.65f, -0.25f, 0f), new Vector3(4.7f, 0.5f, 10f), floor);
        Box("01 enlarged jump pit", new Vector3(14f, -2.4f, 0f), new Vector3(4.6f, 4.4f, 10f), dark, false);
        AddJumpPad("01 A - F/X auto jump", new Vector3(11.35f, 0.12f, 0f), "A", blue);
        AddJumpPad("01 B - auto landing", new Vector3(16.65f, 0.12f, 0f), "B", blue);
        Pickup("01 blue ability pickup", new Vector3(9.3f, 0.7f, 2.5f), blue, "파랑 능력 · 접촉 획득");
        Label("01  파랑 점프 학습", new Vector3(14f, 3.5f, 3.7f), white, 0.25f);
        Label("넓은 단일 개구부 · 옆 우회 불가", new Vector3(14f, 2.85f, 3.7f), blue, 0.16f);
        Label("A에서 F/X → B까지 자동 도약", new Vector3(14f, 1.3f, -0.7f), white, 0.15f);
    }

    private static void Build02And03()
    {
        BuildRoomShell("02 G1 - child switch route", 20f, 38f, -5.5f, 5.5f, 2.4f, 2.4f, false, new[] { 21.2f, 36.8f });
        Box("02 west floor before second jump", new Vector3(25f, -0.25f, 0f), new Vector3(10f, 0.5f, 11f), floor);
        Box("02 east floor after second jump", new Vector3(36f, -0.25f, 0f), new Vector3(4f, 0.5f, 11f), floor);
        Box("02 robot jump pit", new Vector3(33f, -2.1f, 0f), new Vector3(6f, 4f, 11f), dark, false);

        // G1 blocks the main route. Only the low child duct reaches SW1 and the B-side waiting nook.
        Gate("G1 - closed main route gate", new Vector3(23.5f, 1.45f, 0f), new Vector3(0.34f, 2.9f, 11f), yellow);
        Box("02 crawl duct floor", new Vector3(29.1f, -0.08f, -6.4f), new Vector3(15.6f, 0.22f, 1.5f), purple);
        Box("02 crawl duct south wall", new Vector3(29.1f, 0.55f, -7.2f), new Vector3(15.6f, 1.1f, 0.18f), blueLight);
        Box("02 crawl duct low roof", new Vector3(29.1f, 1.03f, -6.4f), new Vector3(15.6f, 0.18f, 1.7f), steel);
        Switch("SW1 - child opens G1", new Vector3(25.1f, 0.42f, -6.4f), "SW1", blueLight);
        Label("아이 전용 저상 덕트", new Vector3(29f, 1.9f, -6.4f), purple, 0.17f);
        Label("G1 개방 후 B측 대기", new Vector3(35.5f, 1.85f, -6.4f), purple, 0.14f);

        AddJumpPad("02 A - robot start", new Vector3(29.25f, 0.12f, 0f), "A", blue);
        AddJumpPad("02 B - robot reunion", new Vector3(35.45f, 0.12f, 0f), "B", blue);
        Label("02  잠금 구역", new Vector3(28.5f, 3.5f, 4.1f), white, 0.25f);
        Label("아이: SW1 → 저상 덕트 → B측", new Vector3(28.4f, 2.85f, 4.1f), purple, 0.15f);
        Label("로봇: G1 통과 → A에서 F/X → B", new Vector3(28.5f, 2.55f, 4.1f), blue, 0.15f);

        BuildRoomShell("03 reunion - ability application", 38f, 44f, -5.5f, 5.5f, 2.4f, 2.4f, true);
        Label("03  재합류 · 적용 확인", new Vector3(41f, 3.3f, 0f), white, 0.2f);
        Label("아이와 로봇이 다시 함께 이동", new Vector3(41f, 2.7f, 0f), blue, 0.15f);
        Marker("03 reunion marker", new Vector3(41f, 0.55f, 0f), 0.6f, blue);
    }

    private static void Build04()
    {
        BuildRoomShell("04 barrier preparation", 44f, 51f, -5.5f, 5.5f, 2.4f, 2.4f, true);
        Pickup("04 yellow barrier ability", new Vector3(45.7f, 0.65f, 0f), yellow, "노랑 능력 · 접촉 획득");
        InteractionPoint("04 barrier activation point", new Vector3(49.5f, 0.1f, 0f), yellow, "F/X · 베리어 ON");
        Ring("04 barrier cue ring", new Vector3(49.5f, 0.1f, 0f), 1.5f, yellow);
        Label("04  베리어 준비", new Vector3(47.5f, 3.5f, 3.8f), white, 0.23f);
        Label("노랑 획득 → 지정 지점 F/X", new Vector3(47.5f, 2.85f, 3.8f), yellow, 0.15f);
        Label("독성 구간 전체 베리어 유지", new Vector3(47.5f, 2.55f, 3.8f), yellow, 0.15f);
    }

    private static void Build05()
    {
        BuildRoomShell("05 toxic bridge - large open drop", 51f, 64f, -5.5f, 5.5f, 2.4f, 2.4f, false);
        // Solid approaches only. No walkable side bypass exists around the large opening.
        Box("05 west safe landing", new Vector3(51.6f, -0.25f, 0f), new Vector3(1.2f, 0.5f, 11f), floor);
        Box("05 east safe landing", new Vector3(63.4f, -0.25f, 0f), new Vector3(1.2f, 0.5f, 11f), floor);
        Box("05 toxic basin", new Vector3(57.5f, -3.6f, 0f), new Vector3(11.4f, 0.15f, 10.5f), green, false);
        Box("05 dark open drop", new Vector3(57.5f, -1.6f, 0f), new Vector3(11.5f, 3f, 10.7f), dark, false);
        // Alternating slabs are right/left relative to travel along +X: near/far in this plan view.
        float[] xs = { 53.0f, 55.7f, 58.4f, 61.1f };
        float[] zs = { 2.0f, -2.0f, 2.0f, -2.0f };
        string[] sides = { "RIGHT", "LEFT", "RIGHT", "LEFT" };
        for (int i = 0; i < xs.Length; i++)
        {
            float x = xs[i];
            float z = zs[i];
            Box("05 isolated corroded slab " + (i + 1) + " - " + sides[i], new Vector3(x, 0.05f, z), new Vector3(2.25f, 0.3f, 2.45f), steel);
            Cylinder("05 narrow rusted support pillar " + (i + 1), new Vector3(x, -1.18f, z), new Vector3(0.26f, 2.2f, 0.26f), rust);
            Box("05 unstable slab footing " + (i + 1), new Vector3(x, -2.32f, z), new Vector3(0.9f, 0.16f, 0.9f), concrete, false);
            Label((i + 1) + "  " + sides[i], new Vector3(x, 0.9f, z), white, 0.13f);
        }
        // Rusted outer rim walls make the pit read as an unavoidable open chasm.
        Box("05 rust rim north", new Vector3(57.5f, 1.45f, 5.42f), new Vector3(11.5f, 2.9f, 0.2f), rust);
        Box("05 rust rim south", new Vector3(57.5f, 1.45f, -5.42f), new Vector3(11.5f, 2.9f, 0.2f), rust);
        Label("05  독성 낙하 구역", new Vector3(57.5f, 3.45f, 4.3f), white, 0.24f);
        Label("큰 개구부 · 진행: 오른쪽 → 왼쪽 → 오른쪽 → 왼쪽", new Vector3(57.5f, 2.8f, 4.3f), yellow, 0.13f);
        Label("베리어 유지 · 독성 중 아이 하차 불가", new Vector3(57.5f, 2.52f, 4.3f), white, 0.13f);
        Label("발판 아래 가는 부식 기둥으로 불안정성 표시", new Vector3(57.5f, -0.1f, -4.2f), greenLight, 0.13f);
    }

    private static void Build06And08()
    {
        BuildRoomShell("06 red ability lock room", 64f, 77f, -6f, 6f, 2.4f, 2.4f, false, new[] { 66.2f }, new[] { 72.1f }, 1.5f, 3.5f);
        Box("06 main route floor", new Vector3(70.5f, -0.25f, 0f), new Vector3(13f, 0.5f, 12f), floor);
        // A low side duct gives the child a brief solo task and returns to the robot immediately.
        Box("06 crawl route floor", new Vector3(67.5f, -0.08f, -7.0f), new Vector3(3.5f, 0.22f, 1.5f), purple);
        Box("06 crawl route south wall", new Vector3(67.5f, 0.55f, -7.8f), new Vector3(3.5f, 1.1f, 0.18f), blueLight);
        Box("06 crawl route low roof", new Vector3(67.5f, 1.03f, -7.0f), new Vector3(3.5f, 0.18f, 1.7f), steel);
        Switch("SW3 - child opens H3", new Vector3(68.6f, 0.42f, -7.0f), "SW3", blueLight);
        Label("아이 전용 · 짧은 왕복 덕트", new Vector3(69.4f, 1.9f, -7.0f), purple, 0.14f);

        // H3 protects the red ability pickup in a side alcove; it does not block the main route.
        Box("06 red alcove floor", new Vector3(72.1f, -0.1f, 7.7f), new Vector3(5.8f, 0.3f, 4.0f), concrete);
        Box("06 red alcove west wall", new Vector3(69.2f, 1.35f, 7.7f), new Vector3(0.3f, 2.7f, 4.0f), wall);
        Box("06 red alcove east wall", new Vector3(75.0f, 1.35f, 7.7f), new Vector3(0.3f, 2.7f, 4.0f), wall);
        Box("06 red alcove end wall", new Vector3(72.1f, 1.35f, 9.7f), new Vector3(5.8f, 2.7f, 0.3f), wall);
        Box("06 H3 gate", new Vector3(72.1f, 1.35f, 5.95f), new Vector3(3.5f, 2.7f, 0.28f), yellow);
        Box("06 H3 gate left jamb", new Vector3(70.05f, 1.45f, 5.95f), new Vector3(0.3f, 2.9f, 0.5f), wall);
        Box("06 H3 gate right jamb", new Vector3(74.15f, 1.45f, 5.95f), new Vector3(0.3f, 2.9f, 0.5f), wall);
        Label("H3 잠금", new Vector3(72.1f, 3.1f, 5.25f), yellow, 0.18f);
        Pickup("06 red cube behind H3", new Vector3(72.1f, 0.7f, 8.1f), red, "빨강 파괴 능력");
        Label("SW3 해제 후 빨강 획득", new Vector3(72.1f, 2.4f, 8.7f), red, 0.14f);

        // W1 blocks the required exit until the robot uses the red ability.
        BreakableWall("W1 - required exit wall", new Vector3(76.8f, 1.45f, 0f), new Vector3(0.4f, 2.9f, 12f), "W1  F/X");
        Label("06  잠금실 · 빨강 능력", new Vector3(70.1f, 3.7f, 2.8f), white, 0.24f);
        Label("아이 SW3 → H3 열기 → 빨강 큐브", new Vector3(70.1f, 3.05f, 2.8f), yellow, 0.14f);
        Label("로봇이 W1을 파괴해 필수 출구 개방", new Vector3(70.1f, 2.78f, 2.8f), red, 0.13f);

        // Optional W2 branch with the single environmental family-photo discovery.
        Box("08 branch floor", new Vector3(72.3f, -0.23f, -12.1f), new Vector3(6.2f, 0.46f, 8.2f), floor);
        Box("08 connector floor", new Vector3(72.3f, -0.23f, -8.8f), new Vector3(3.0f, 0.46f, 3.5f), floor);
        Box("08 branch side wall left", new Vector3(69.2f, 1.35f, -12.1f), new Vector3(0.25f, 2.7f, 8.2f), wall);
        Box("08 branch side wall right", new Vector3(75.4f, 1.35f, -12.1f), new Vector3(0.25f, 2.7f, 8.2f), wall);
        Box("08 branch end wall left", new Vector3(72.3f, 1.35f, -16.1f), new Vector3(6.2f, 2.7f, 0.25f), wall);
        BreakableWall("W2 - optional discovery wall", new Vector3(72.3f, 1.35f, -9.0f), new Vector3(3f, 2.7f, 0.35f), "W2  선택");
        PhotoFrame(new Vector3(72.3f, 1.2f, -14.8f));
        Label("08  선택 발견물", new Vector3(72.3f, 3.3f, -13f), white, 0.18f);
        Label("가족 사진 · 필수 진행과 무관", new Vector3(72.3f, 2.8f, -13f), yellow, 0.13f);
    }

    private static void Build07And09()
    {
        BuildRoomShell("07 reunion", 77f, 83f, -6f, 6f, 2.4f, 2.4f, true);
        Label("07  재합류", new Vector3(80f, 3.2f, 0f), white, 0.22f);
        Label("E/B · 어부바 복귀", new Vector3(80f, 2.65f, 0f), blue, 0.15f);
        Marker("07 reunion marker", new Vector3(80f, 0.55f, 0f), 0.6f, blue);

        BuildRoomShell("09 exit", 83f, 91f, -5f, 5f, 2.4f, 2.4f, true);
        Box("09 exit marker", new Vector3(88f, 0.12f, 0f), new Vector3(1.2f, 0.24f, 1.2f), yellow);
        Label("09  출구", new Vector3(87f, 3.2f, 0f), white, 0.22f);
        Label("W1 통과 후 도달", new Vector3(87f, 2.65f, 0f), blue, 0.15f);
    }

    private static void BuildRoomShell(string roomName, float xMin, float xMax, float zMin, float zMax, float westDoorWidth, float eastDoorWidth, bool fullFloor, float[] southDoorCenters = null, float[] northDoorCenters = null, float southDoorWidth = 1.5f, float northDoorWidth = 1.5f)
    {
        if (fullFloor)
            Box(roomName + " floor", new Vector3((xMin + xMax) * 0.5f, -0.25f, (zMin + zMax) * 0.5f), new Vector3(xMax - xMin, 0.5f, zMax - zMin), floor);

        float h = 3f;
        float t = 0.3f;
        if (northDoorCenters == null || northDoorCenters.Length == 0)
            Box(roomName + " north fixed wall", new Vector3((xMin + xMax) * 0.5f, h * 0.5f, zMax), new Vector3(xMax - xMin, h, t), wall);
        else
            LongWallWithOpenings(roomName + " north", xMin, xMax, zMax, northDoorCenters, northDoorWidth, h, t);
        if (southDoorCenters == null || southDoorCenters.Length == 0)
            Box(roomName + " south fixed wall", new Vector3((xMin + xMax) * 0.5f, h * 0.5f, zMin), new Vector3(xMax - xMin, h, t), wall);
        else
            LongWallWithOpenings(roomName + " south", xMin, xMax, zMin, southDoorCenters, southDoorWidth, h, t);
        EndWallWithDoor(roomName + " west", xMin, zMin, zMax, westDoorWidth, h, t);
        EndWallWithDoor(roomName + " east", xMax, zMin, zMax, eastDoorWidth, h, t);
    }

    private static void LongWallWithOpenings(string name, float xMin, float xMax, float z, float[] centers, float openingWidth, float height, float thickness)
    {
        System.Array.Sort(centers);
        float cursor = xMin;
        for (int i = 0; i < centers.Length; i++)
        {
            float left = Mathf.Clamp(centers[i] - openingWidth * 0.5f, cursor, xMax);
            float right = Mathf.Clamp(centers[i] + openingWidth * 0.5f, left, xMax);
            if (left > cursor)
                Box(name + " wall segment " + i, new Vector3((cursor + left) * 0.5f, height * 0.5f, z), new Vector3(left - cursor, height, thickness), wall);
            Box(name + " opening lintel " + i, new Vector3((left + right) * 0.5f, height - 0.18f, z), new Vector3(right - left, 0.36f, thickness), steel);
            cursor = right;
        }
        if (cursor < xMax)
            Box(name + " wall segment end", new Vector3((cursor + xMax) * 0.5f, height * 0.5f, z), new Vector3(xMax - cursor, height, thickness), wall);
    }

    private static void EndWallWithDoor(string name, float x, float zMin, float zMax, float doorWidth, float height, float thickness)
    {
        float center = (zMin + zMax) * 0.5f;
        float leftEnd = center - doorWidth * 0.5f;
        float rightStart = center + doorWidth * 0.5f;
        if (leftEnd > zMin)
            Box(name + " wall segment A", new Vector3(x, height * 0.5f, (zMin + leftEnd) * 0.5f), new Vector3(thickness, height, leftEnd - zMin), wall);
        if (zMax > rightStart)
            Box(name + " wall segment B", new Vector3(x, height * 0.5f, (rightStart + zMax) * 0.5f), new Vector3(thickness, height, zMax - rightStart), wall);
        Box(name + " doorway lintel", new Vector3(x, height - 0.18f, center), new Vector3(thickness, 0.36f, doorWidth), steel);
    }

    private static void Gate(string name, Vector3 position, Vector3 size, Material material)
    {
        Box(name, position, size, material);
        Label(name, position + Vector3.up * 1.9f, material == null ? white : yellow, 0.16f);
        Box(name + " frame left", position + new Vector3(0f, 0f, -size.z * 0.5f), new Vector3(size.x + 0.15f, size.y, 0.2f), wall);
        Box(name + " frame right", position + new Vector3(0f, 0f, size.z * 0.5f), new Vector3(size.x + 0.15f, size.y, 0.2f), wall);
    }

    private static void AddJumpPad(string name, Vector3 position, string label, Material mat)
    {
        Box(name + " pad", position, new Vector3(1.35f, 0.22f, 2.0f), mat);
        Box(name + " marker face", position + Vector3.up * 0.16f, new Vector3(0.95f, 0.08f, 1.55f), blueLight);
        Label(label, position + Vector3.up * 0.75f, white, 0.28f);
        Label("F/X", position + new Vector3(0f, 0.48f, 0.95f), blue, 0.12f);
    }

    private static void Pickup(string name, Vector3 position, Material mat, string caption)
    {
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Cube);
        item.name = name;
        item.transform.SetParent(root, false);
        item.transform.position = position;
        item.transform.localScale = Vector3.one * 0.8f;
        item.transform.rotation = Quaternion.Euler(45f, 45f, 45f);
        item.GetComponent<Renderer>().sharedMaterial = mat;
        Label(caption, position + Vector3.up * 0.9f, mat, 0.14f);
    }

    private static void InteractionPoint(string name, Vector3 position, Material mat, string caption)
    {
        Cylinder(name + " pad", position + Vector3.down * 0.03f, new Vector3(1.25f, 0.06f, 1.25f), mat);
        Label(caption, position + Vector3.up * 1.0f, mat, 0.16f);
    }

    private static void Switch(string name, Vector3 position, string label, Material mat)
    {
        Box(name + " pedestal", position, new Vector3(0.8f, 0.8f, 0.65f), concrete);
        Box(name + " button", position + Vector3.up * 0.45f, new Vector3(0.42f, 0.12f, 0.42f), mat);
        Label(label + " · 아이 조작", position + Vector3.up * 1.05f, mat, 0.13f);
    }

    private static void BreakableWall(string name, Vector3 position, Vector3 size, string caption)
    {
        Box(name + " blocked collider mass", position, size, dark);
        int count = Mathf.Max(2, Mathf.RoundToInt(size.z / 1.2f));
        for (int i = 0; i < count; i++)
        {
            float z = position.z - size.z * 0.5f + (i + 0.5f) * size.z / count;
            float y = 0.55f + (i % 2) * 0.65f;
            Box(name + " red break block " + (i + 1), new Vector3(position.x - 0.04f, y, z), new Vector3(size.x + 0.08f, 0.6f, size.z / count - 0.06f), red);
        }
        Box(name + " top frame", position + Vector3.up * size.y * 0.5f, new Vector3(size.x + 0.2f, 0.25f, size.z + 0.2f), rust);
        Label(caption, position + Vector3.up * (size.y * 0.5f + 0.4f), red, 0.17f);
    }

    private static void PhotoFrame(Vector3 position)
    {
        Box("08 family photo backboard", position, new Vector3(2.4f, 1.7f, 0.18f), rust);
        Box("08 photo panel", position + new Vector3(0f, 0f, -0.12f), new Vector3(1.9f, 1.3f, 0.08f), white);
        Box("08 photo landscape", position + new Vector3(0f, -0.2f, -0.18f), new Vector3(1.65f, 0.52f, 0.04f), greenLight);
        Sphere("08 photo figure child", position + new Vector3(-0.35f, 0.28f, -0.23f), 0.22f, blueLight);
        Sphere("08 photo figure parent", position + new Vector3(0.3f, 0.3f, -0.23f), 0.26f, yellow);
        Label("가족 사진 · 단일 발견물", position + Vector3.up * 1.35f, white, 0.13f);
    }

    private static void Marker(string name, Vector3 position, float height, Material mat)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = name;
        go.transform.SetParent(root, false);
        go.transform.position = position;
        go.transform.localScale = new Vector3(0.55f, height, 0.55f);
        go.GetComponent<Renderer>().sharedMaterial = mat;
    }

    private static void RobotPlaceholder(string name, Vector3 position)
    {
        Box(name + " body", position, new Vector3(0.8f, 1.4f, 0.65f), steel);
        Sphere(name + " head", position + Vector3.up * 0.95f, 0.42f, dark);
        Box(name + " shoulder", position + new Vector3(0f, 0.5f, -0.35f), new Vector3(1.15f, 0.25f, 0.32f), rust);
    }

    private static void Ring(string name, Vector3 position, float diameter, Material mat)
    {
        Cylinder(name, position + Vector3.up * 0.03f, new Vector3(diameter, 0.06f, diameter), mat);
        Cylinder(name + " inner floor", position + Vector3.up * 0.07f, new Vector3(diameter * 0.72f, 0.04f, diameter * 0.72f), floor);
    }

    private static void BuildWorldBase()
    {
        // Low, neutral foundation keeps the full graybox readable in the overview camera.
        Box("World foundation", new Vector3(45.5f, -4.3f, -4f), new Vector3(100f, 0.4f, 34f), dark, false);
        Box("World north boundary", new Vector3(45.5f, -2.3f, 16.5f), new Vector3(100f, 4f, 0.35f), wall);
    }

    private static void AddNorthAndScaleLegend()
    {
        Label("진행 방향  →  +X", new Vector3(45f, 4.2f, 14f), white, 0.22f);
        Label("블록아웃 기준 · 1 Unity Unit ≈ 1m (검증 전)", new Vector3(45f, 3.65f, 14f), white, 0.15f);
        Box("north marker stem", new Vector3(2.2f, 0.1f, 11.8f), new Vector3(0.12f, 0.2f, 1.6f), white, false);
        Label("N", new Vector3(2.2f, 0.75f, 12.8f), white, 0.18f);
        Label("필수 경로", new Vector3(5.2f, 3.1f, 12.2f), blue, 0.13f);
        Label("아이 전용", new Vector3(5.2f, 2.75f, 12.2f), purple, 0.13f);
        Label("고정 차단 / 파괴벽", new Vector3(5.2f, 2.4f, 12.2f), red, 0.13f);
    }

    private static Camera CreateOverviewCamera()
    {
        GameObject go = new GameObject("POC Overview Camera");
        go.transform.SetParent(root, false);
        go.transform.position = new Vector3(45.5f, 64f, -62f);
        go.transform.LookAt(new Vector3(45.5f, 0f, 0f));
        Camera cam = go.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 30f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.12f, 0.15f, 0.17f);
        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 180f;
        go.tag = "MainCamera";
        return cam;
    }

    private static void CreateLighting()
    {
        GameObject sun = new GameObject("Graybox key light");
        sun.transform.SetParent(root, false);
        sun.transform.rotation = Quaternion.Euler(48f, -28f, 0f);
        Light light = sun.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
        light.color = new Color(1f, 0.94f, 0.82f);
        RenderSettings.ambientLight = new Color(0.48f, 0.51f, 0.54f);
    }

    private static GameObject Box(string name, Vector3 position, Vector3 size, Material mat, bool collider = true)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(root, false);
        go.transform.position = position;
        go.transform.localScale = size;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        if (!collider)
            Object.DestroyImmediate(go.GetComponent<Collider>());
        return go;
    }

    private static GameObject Cylinder(string name, Vector3 position, Vector3 scale, Material mat)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = name;
        go.transform.SetParent(root, false);
        go.transform.position = position;
        // Unity's built-in cylinder mesh is two units high; convert requested world height to scale.
        go.transform.localScale = new Vector3(scale.x, scale.y * 0.5f, scale.z);
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }

    private static GameObject Sphere(string name, Vector3 position, float diameter, Material mat)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        go.transform.SetParent(root, false);
        go.transform.position = position;
        go.transform.localScale = Vector3.one * diameter;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }

    private static void Label(string text, Vector3 position, Material colorMaterial, float characterSize)
    {
        GameObject go = new GameObject("Label - " + text);
        go.transform.SetParent(root, false);
        go.transform.position = position;
        go.transform.rotation = Quaternion.LookRotation(overviewCamera.transform.position - position, Vector3.up);
        TextMesh mesh = go.AddComponent<TextMesh>();
        mesh.text = text;
        mesh.font = labelFont;
        mesh.fontSize = 64;
        mesh.characterSize = characterSize;
        mesh.anchor = TextAnchor.MiddleCenter;
        mesh.alignment = TextAlignment.Center;
        mesh.color = colorMaterial != null ? colorMaterial.color : Color.white;
        mesh.richText = false;
        mesh.GetComponent<MeshRenderer>().sortingOrder = 10;
    }

    private static Material MakeMaterial(string name, Color color)
    {
        string path = MaterialsPath + "/" + name.Replace(" ", "_") + ".mat";
        Material existing = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (existing != null)
        {
            existing.color = color;
            existing.SetColor("_BaseColor", color);
            EditorUtility.SetDirty(existing);
            return existing;
        }
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null)
            shader = Shader.Find("Standard");
        Material material = new Material(shader) { name = name, color = color };
        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", color);
        AssetDatabase.CreateAsset(material, path);
        return material;
    }

    private static Font LoadLabelFont()
    {
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font == null)
            font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        return font;
    }

    private static void EnsureFolder(string parent, string folder)
    {
        string path = parent + "/" + folder;
        if (!AssetDatabase.IsValidFolder(path))
            AssetDatabase.CreateFolder(parent, folder);
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        EditorBuildSettingsScene[] oldScenes = EditorBuildSettings.scenes;
        foreach (EditorBuildSettingsScene oldScene in oldScenes)
            if (oldScene.path == scenePath)
                return;

        EditorBuildSettingsScene[] newScenes = new EditorBuildSettingsScene[oldScenes.Length + 1];
        newScenes[0] = new EditorBuildSettingsScene(scenePath, true);
        for (int i = 0; i < oldScenes.Length; i++)
            newScenes[i + 1] = oldScenes[i];
        EditorBuildSettings.scenes = newScenes;
    }

}
