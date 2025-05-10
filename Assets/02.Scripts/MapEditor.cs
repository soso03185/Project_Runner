using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System;

public class MapEditor : EditorWindow
{
    [Serializable]
    public class PalettePrefab
    {
        public GameObject prefab;
        public Color color = Color.yellow;
        public float height = 0f;
    }

    [Serializable]
    public class ObjectPrefab
    {
        public GameObject prefab;
        public ObjectType type = ObjectType.Coin;
        public float height = 0f;
    }

    public enum ObjectType { Coin, Monster }

    private List<PalettePrefab> palettePrefabs = new();
    private List<ObjectPrefab> objectPrefabs = new();
    private int selectedPaletteIndex = -1;
    private int selectedObjectIndex = -1;

    private class TileData
    {
        public int paletteIndex = -1;
        public GameObject instance;
    }

    private class ObjectTileData
    {
        public int objectIndex = -1;
        public GameObject instance;
    }

    private TileData[,] placedTiles;
    private ObjectTileData[,] placedObjects;

    private const float cellSize = 24f;
    private Vector2 scrollPos;
    private Vector2 paletteScroll = Vector2.zero;
    private Vector2 objectScroll = Vector2.zero;
    private Vector2 mainScroll = Vector2.zero;
    private bool isMouseDown = false;
    private bool isRightMouseDown = false;
    private Vector2Int? lastHoveredCell = null;
    private bool moveLeft, moveRight;
    private const float gridPadding = 15f;

    private int gridSizeX = 3;
    private int gridSizeZ = 16;

    private enum Mode { Palette, Object }
    private Mode currentMode = Mode.Palette;

    [MenuItem("Tools/Map Editor")]
    public static void OpenWindow()
    {
        GetWindow<MapEditor>("Map Editor");
    }

    private void OnEnable()
    {
        placedTiles = new TileData[gridSizeX, gridSizeZ];
        placedObjects = new ObjectTileData[gridSizeX, gridSizeZ];
        EditorApplication.update += AutoScroll;
    }

    private void OnDisable()
    {
        EditorApplication.update -= AutoScroll;
    }

    private void AutoScroll()
    {
        bool changed = false;

        if (moveLeft) { scrollPos.x -= 6f; changed = true; }
        if (moveRight) { scrollPos.x += 6f; changed = true; }

        if (changed)
        {
            Repaint();
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            SaveMapData();
        }
        if (GUILayout.Button("Load", GUILayout.Height(30)))
        {
            LoadMapData();
        }

        GUILayout.EndHorizontal();

        mainScroll = EditorGUILayout.BeginScrollView(
               mainScroll,
               alwaysShowHorizontal: false,
               alwaysShowVertical: true // <- 이게 핵심
           );

        GUILayout.Space(15);
        GUILayout.Label("타일 배치 도구", EditorStyles.boldLabel);

        int newGridSizeX = EditorGUILayout.IntField("그리드 너비 (X)", Math.Max(1, gridSizeX));
        int newGridSizeZ = EditorGUILayout.IntField("그리드 깊이 (Z)", Math.Max(1, gridSizeZ));

        if (newGridSizeX != gridSizeX || newGridSizeZ != gridSizeZ)
        {
            TileData[,] newPlacedTiles = new TileData[newGridSizeX, newGridSizeZ];
            ObjectTileData[,] newPlacedObjects = new ObjectTileData[newGridSizeX, newGridSizeZ];
            for (int x = 0; x < Mathf.Min(gridSizeX, newGridSizeX); x++)
            {
                for (int z = 0; z < Mathf.Min(gridSizeZ, newGridSizeZ); z++)
                {
                    newPlacedTiles[x, z] = placedTiles[x, z];
                    newPlacedObjects[x, z] = placedObjects[x, z];
                }
            }
            placedTiles = newPlacedTiles;
            placedObjects = newPlacedObjects;
            gridSizeX = newGridSizeX;
            gridSizeZ = newGridSizeZ;
        }

        GUILayout.Space(15);

        GUILayout.Label("팔레트 그리드", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal(GUILayout.Height(150));
        GUILayout.Space(gridPadding);

        // 버튼
        GUILayout.BeginVertical();
        GUILayout.Space((150 - 90) * 0.5f);
        if (GUILayout.RepeatButton("←", GUILayout.Width(30), GUILayout.Height(90)))
            moveLeft = true;
        else if (Event.current.type == EventType.Repaint)
            moveLeft = false;
        GUILayout.EndVertical();

        // ScrollView  유동 크기, 최소 너비 보장
        float minScrollWidth = 100; // 최소 보장할 스크롤 뷰 너비
        float usableWidth = position.width - gridPadding * 2 - 30 * 2; // 전체  좌우 버튼 너비
        float scrollWidth = Mathf.Max(minScrollWidth, usableWidth);

        scrollPos = EditorGUILayout.BeginScrollView(
            scrollPos,
            alwaysShowHorizontal: true,
            alwaysShowVertical: false,
            GUILayout.Width(scrollWidth),
            GUILayout.Height(150)
        );

        float totalWidth = gridSizeZ * cellSize;
        float totalHeight = gridSizeX * cellSize;
        float gridInnerPadding = 25f;

        Rect gridRect = GUILayoutUtility.GetRect(totalWidth, totalHeight + cellSize);
        float tileStartY = gridRect.y + gridInnerPadding;

        Rect fillBackground = new Rect(gridRect.x, tileStartY, totalWidth, totalHeight);
        EditorGUI.DrawRect(fillBackground, new Color(0.15f, 0.15f, 0.15f));

        Color cellBorderColor = new Color(0.3f, 0.3f, 0.3f, 1f);

        Event e = Event.current;
        Vector2 mousePos = e.mousePosition;
        bool mouseInGrid = gridRect.Contains(mousePos);

        // 그리드 내부의 Z축 숫자
        for (int z = 0; z < gridSizeZ; z++)
        {
            Rect labelRect = new Rect(gridRect.x + z * cellSize, tileStartY - 16, cellSize, 16);
            GUI.Label(labelRect, z.ToString(), EditorStyles.centeredGreyMiniLabel);
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Rect cellRect = new Rect(gridRect.x + z * cellSize, tileStartY + x * cellSize, cellSize, cellSize);

                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, cellRect.width, 1), cellBorderColor);
                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.yMax - 1, cellRect.width, 1), cellBorderColor);
                EditorGUI.DrawRect(new Rect(cellRect.x, cellRect.y, 1, cellRect.height), cellBorderColor);
                EditorGUI.DrawRect(new Rect(cellRect.xMax - 1, cellRect.y, 1, cellRect.height), cellBorderColor);

                if (placedTiles[x, z]?.paletteIndex >= 0 && placedTiles[x, z].paletteIndex < palettePrefabs.Count)
                {
                    var palette = palettePrefabs[placedTiles[x, z].paletteIndex];
                    if (palette.prefab != null)
                    {
                        Rect fill = new Rect(cellRect.x + 2, cellRect.y + 2, cellSize - 4, cellSize - 4);
                        EditorGUI.DrawRect(fill, palette.color);
                    }
                }

                if (placedObjects[x, z]?.objectIndex >= 0 && placedObjects[x, z].objectIndex < objectPrefabs.Count)
                {
                    var obj = objectPrefabs[placedObjects[x, z].objectIndex];
                    if (obj.prefab != null)
                    {
                        GUI.Label(cellRect, obj.type == ObjectType.Coin ? "c" : "m", EditorStyles.centeredGreyMiniLabel);
                    }
                }

                if (mouseInGrid && cellRect.Contains(mousePos))
                {
                    if (e.type == EventType.MouseDown || e.type == EventType.MouseDrag)
                    {
                        if (e.button == 0)
                        {
                            isMouseDown = true;
                            if (currentMode == Mode.Palette && selectedPaletteIndex >= 0 && selectedPaletteIndex < palettePrefabs.Count)
                            {
                                var palette = palettePrefabs[selectedPaletteIndex];
                                if (palette.prefab != null)
                                {
                                    if (placedTiles[x, z]?.instance != null)
                                        DestroyImmediate(placedTiles[x, z].instance);

                                    var instance = (GameObject)PrefabUtility.InstantiatePrefab(palette.prefab);
                                    instance.transform.position = new Vector3(x, palette.height, z);
                                    placedTiles[x, z] = new TileData { paletteIndex = selectedPaletteIndex, instance = instance };
                                }
                            }
                            else if (currentMode == Mode.Object && selectedObjectIndex >= 0 && selectedObjectIndex < objectPrefabs.Count)
                            {
                                var obj = objectPrefabs[selectedObjectIndex];
                                if (obj.prefab != null)
                                {
                                    if (placedObjects[x, z]?.instance != null)
                                        DestroyImmediate(placedObjects[x, z].instance);

                                    var instance = (GameObject)PrefabUtility.InstantiatePrefab(obj.prefab);
                                    instance.transform.position = new Vector3(x, obj.height, z);
                                    placedObjects[x, z] = new ObjectTileData { objectIndex = selectedObjectIndex, instance = instance };
                                }
                            }
                            e.Use();
                        }
                        else if (e.button == 1)
                        {
                            isRightMouseDown = true;

                            if (currentMode == Mode.Palette)
                            {
                                if (placedTiles[x, z] != null)
                                {
                                    if (placedTiles[x, z].instance != null)
                                        DestroyImmediate(placedTiles[x, z].instance);

                                    placedTiles[x, z] = null;
                                }
                            }
                            else if (currentMode == Mode.Object)
                            {
                                if (placedObjects[x, z] != null)
                                {
                                    if (placedObjects[x, z].instance != null)
                                        DestroyImmediate(placedObjects[x, z].instance);

                                    placedObjects[x, z] = null;
                                }
                            }

                            e.Use();
                        }

                    }
                }
            }
        }

        if (e.type == EventType.MouseUp)
        {
            isMouseDown = false;
            isRightMouseDown = false;
            lastHoveredCell = null;
        }

        EditorGUILayout.EndScrollView();

        // 버튼 – 항상 고정 위치에 보이게
        GUILayout.BeginVertical();
        GUILayout.Space((150 - 90) * 0.5f);
        if (GUILayout.RepeatButton("→", GUILayout.Width(30), GUILayout.Height(90)))
            moveRight = true;
        else if (Event.current.type == EventType.Repaint)
            moveRight = false;
        GUILayout.EndVertical();

        GUILayout.Space(gridPadding);
        GUILayout.EndHorizontal();



        GUILayout.Label("타일 관리", EditorStyles.boldLabel);
        paletteScroll = EditorGUILayout.BeginScrollView(paletteScroll, GUILayout.Height(190));
        GUILayout.BeginHorizontal();

        for (int i = 0; i < palettePrefabs.Count; i++)
        {
            bool isSelected = i == selectedPaletteIndex;
            GUI.backgroundColor = isSelected ? Color.green : Color.white;
            GUILayout.BeginVertical("box", GUILayout.Width(90));
            GUI.backgroundColor = Color.white;

            if (GUILayout.Button("Select", GUILayout.Width(80)))
            {
                selectedPaletteIndex = i;
                currentMode = Mode.Palette;
            }

            Texture2D preview = AssetPreview.GetAssetPreview(palettePrefabs[i].prefab);
            if (preview == null)
                preview = AssetPreview.GetMiniThumbnail(palettePrefabs[i].prefab);

            if (preview != null)
                GUILayout.Label(preview, GUILayout.Width(80), GUILayout.Height(80));
            else
                GUILayout.Box("None", GUILayout.Width(80), GUILayout.Height(80));

            palettePrefabs[i].prefab = (GameObject)EditorGUILayout.ObjectField(palettePrefabs[i].prefab, typeof(GameObject), false, GUILayout.Width(80));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Ypos", GUILayout.Width(35));
            palettePrefabs[i].height = EditorGUILayout.FloatField(GUIContent.none, palettePrefabs[i].height, GUILayout.Width(40));
            GUILayout.EndHorizontal();

            GUILayout.Space(4);
            palettePrefabs[i].color = EditorGUILayout.ColorField(GUIContent.none, palettePrefabs[i].color, GUILayout.Width(80));

            GUILayout.EndVertical();
        }

        GUI.backgroundColor = Color.white;

        GUILayout.BeginVertical();
        if (GUILayout.Button("ADD", GUILayout.Height(40), GUILayout.Width(60)))
        {
            palettePrefabs.Add(new PalettePrefab());
            selectedPaletteIndex = palettePrefabs.Count - 1;
        }
        if (GUILayout.Button("DEL", GUILayout.Height(40), GUILayout.Width(60)))
        {
            if (selectedPaletteIndex >= 0 && selectedPaletteIndex < palettePrefabs.Count)
            {
                palettePrefabs.RemoveAt(selectedPaletteIndex);
                selectedPaletteIndex = -1;
            }
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        GUILayout.Label("오브젝트 관리", EditorStyles.boldLabel);
        objectScroll = EditorGUILayout.BeginScrollView(objectScroll, GUILayout.Height(190));
        GUILayout.BeginHorizontal();

        for (int i = 0; i < objectPrefabs.Count; i++)
        {
            bool isSelected = i == selectedObjectIndex;
            GUI.backgroundColor = isSelected ? Color.cyan : Color.white;
            GUILayout.BeginVertical("box", GUILayout.Width(90));
            GUI.backgroundColor = Color.white;

            if (GUILayout.Button("Select", GUILayout.Width(80)))
            {
                selectedObjectIndex = i;
                currentMode = Mode.Object;
            }

            Texture2D preview = AssetPreview.GetAssetPreview(objectPrefabs[i].prefab);
            if (preview == null)
                preview = AssetPreview.GetMiniThumbnail(objectPrefabs[i].prefab);

            if (preview != null)
                GUILayout.Label(preview, GUILayout.Width(80), GUILayout.Height(80));
            else
                GUILayout.Box("None", GUILayout.Width(80), GUILayout.Height(80));

            objectPrefabs[i].prefab = (GameObject)EditorGUILayout.ObjectField(objectPrefabs[i].prefab, typeof(GameObject), false, GUILayout.Width(80));

            objectPrefabs[i].type = (ObjectType)EditorGUILayout.EnumPopup(objectPrefabs[i].type);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Ypos", GUILayout.Width(35));
            objectPrefabs[i].height = EditorGUILayout.FloatField(GUIContent.none, objectPrefabs[i].height, GUILayout.Width(40));
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        GUI.backgroundColor = Color.white;

        GUILayout.BeginVertical();
        if (GUILayout.Button("ADD", GUILayout.Height(40), GUILayout.Width(60)))
        {
            objectPrefabs.Add(new ObjectPrefab());
            selectedObjectIndex = objectPrefabs.Count - 1;
        }
        if (GUILayout.Button("DEL", GUILayout.Height(40), GUILayout.Width(60)))
        {
            if (selectedObjectIndex >= 0 && selectedObjectIndex < objectPrefabs.Count)
            {
                objectPrefabs.RemoveAt(selectedObjectIndex);
                selectedObjectIndex = -1;
            }
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndScrollView();
    }


    private void SaveMapData()
    {
        var data = new MapSaveData
        {
            gridSizeX = gridSizeX,
            gridSizeZ = gridSizeZ
        };

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                var tile = placedTiles[x, z];
                if (tile != null && tile.instance != null && tile.paletteIndex >= 0 && tile.paletteIndex < palettePrefabs.Count)
                {
                    var palette = palettePrefabs[tile.paletteIndex];
                    string prefabPath = AssetDatabase.GetAssetPath(palette.prefab);
                    data.tiles.Add(new TileSaveData
                    {
                        x = x,
                        z = z,
                        prefabPath = prefabPath,
                        position = tile.instance.transform.position,
                        color = palette.color
                    });
                }

                var objTile = placedObjects[x, z];
                if (objTile != null && objTile.instance != null && objTile.objectIndex >= 0 && objTile.objectIndex < objectPrefabs.Count)
                {
                    var obj = objectPrefabs[objTile.objectIndex];
                    string prefabPath = AssetDatabase.GetAssetPath(obj.prefab);
                    data.objects.Add(new ObjectSaveData
                    {
                        x = x,
                        z = z,
                        prefabPath = prefabPath,
                        position = objTile.instance.transform.position,
                        objectType = obj.type.ToString()
                    });
                }
            }
        }

        string json = JsonUtility.ToJson(data, true);
        string path = EditorUtility.SaveFilePanel("Save Map Data", Application.dataPath, "mapData", "json");
        if (!string.IsNullOrEmpty(path))
        {
            System.IO.File.WriteAllText(path, json);
            Debug.Log("Map saved to " + path);
        }
    }

    private void LoadMapData()
    {
        ClearSceneTilesAndObjects();

        string path = EditorUtility.OpenFilePanel("Load Map Data", Application.dataPath, "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = System.IO.File.ReadAllText(path);
        var data = JsonUtility.FromJson<MapSaveData>(json);
        if (data == null)
        {
            Debug.LogError("Failed to load map data");
            return;
        }

        gridSizeX = data.gridSizeX;
        gridSizeZ = data.gridSizeZ;
        placedTiles = new TileData[gridSizeX, gridSizeZ];
        placedObjects = new ObjectTileData[gridSizeX, gridSizeZ];

        // Tile 복원
        foreach (var tile in data.tiles)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(tile.prefabPath);
            if (prefab != null)
            {
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.transform.position = tile.position;

                int index = palettePrefabs.FindIndex(p => AssetDatabase.GetAssetPath(p.prefab) == tile.prefabPath);
                if (index == -1)
                {
                    palettePrefabs.Add(new PalettePrefab
                    {
                        prefab = prefab,
                        color = tile.color,
                        height = tile.position.y
                    });
                    index = palettePrefabs.Count - 1;
                }

                placedTiles[tile.x, tile.z] = new TileData
                {
                    paletteIndex = index,
                    instance = instance
                };
            }
        }

        // Object 복원
        foreach (var obj in data.objects)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(obj.prefabPath);
            if (prefab != null && Enum.TryParse(obj.objectType, out ObjectType type))
            {
                var instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                instance.transform.position = obj.position;

                int index = objectPrefabs.FindIndex(o => AssetDatabase.GetAssetPath(o.prefab) == obj.prefabPath && o.type == type);
                if (index == -1)
                {
                    objectPrefabs.Add(new ObjectPrefab
                    {
                        prefab = prefab,
                        type = type,
                        height = obj.position.y
                    });
                    index = objectPrefabs.Count - 1;
                }

                placedObjects[obj.x, obj.z] = new ObjectTileData
                {
                    objectIndex = index,
                    instance = instance
                };
            }
        }

        Debug.Log("Map loaded from " + path);
    }

    private void ClearSceneTilesAndObjects()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (placedTiles?[x, z]?.instance != null)
                {
                    DestroyImmediate(placedTiles[x, z].instance);
                    placedTiles[x, z] = null;
                }

                if (placedObjects?[x, z]?.instance != null)
                {
                    DestroyImmediate(placedObjects[x, z].instance);
                    placedObjects[x, z] = null;
                }
            }
        }
    }


    [Serializable]
    private class TileSaveData
    {
        public int x, z;
        public string prefabPath;
        public Vector3 position;
        public Color color;
    }

    [Serializable]
    private class ObjectSaveData
    {
        public int x, z;
        public string prefabPath;
        public Vector3 position;
        public string objectType;
    }

    [Serializable]
    private class MapSaveData
    {
        public int gridSizeX;
        public int gridSizeZ;
        public List<TileSaveData> tiles = new();
        public List<ObjectSaveData> objects = new();
    }
}
