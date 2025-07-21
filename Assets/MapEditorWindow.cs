using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class MapEditorWindow : EditorWindow
{
    private enum EditMode { Preset, Object }
    private EditMode currentMode = EditMode.Preset;

    private List<string> prefabNames = new List<string>();
    private int selectedIndex = -1;
    private GameObject selectedPrefab = null;

    private Vector2 scrollPos;
    private Transform mapRoot;

    private const string presetPath = "Prefabs/Map";
    private const string objectPath = "Prefabs/MapObject";

    // 오브젝트 배치 옵션
    private int selectedLane = 0;
    private float objectHeight = 0f;
    private float objectYRotation = 0f;

    [MenuItem("Tools/Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<MapEditorWindow>("Map Editor");
    }

    private void OnEnable()
    {
        LoadPrefabs();
    }

    private void OnGUI()
    {
        DrawModeToggle();
        EditorGUILayout.Space();

        GUILayout.Label("프리팹 목록", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(200), GUILayout.Height(300));
        for (int i = 0; i < prefabNames.Count; i++)
        {
            if (GUILayout.Button(prefabNames[i], (i == selectedIndex) ? EditorStyles.toolbarButton : GUI.skin.button))
            {
                SelectPrefab(i);
            }
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginVertical("box");

        if (selectedPrefab != null)
        {
            GUILayout.Label("선택된 프리팹", EditorStyles.boldLabel);
            GUILayout.Label(selectedPrefab.name);

            EditorGUILayout.Space();

            if (currentMode == EditMode.Object)
            {
                DrawObjectPlacementOptions();
            }

            if (GUILayout.Button("배치하기"))
            {
                if (currentMode == EditMode.Preset)
                    PlacePreset();
                else
                    PlaceMapObject();
            }
        }
        else
        {
            GUILayout.Label("프리팹을 선택해주세요");
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawModeToggle()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(currentMode == EditMode.Preset, "프리셋 모드", EditorStyles.toolbarButton))
        {
            if (currentMode != EditMode.Preset)
            {
                currentMode = EditMode.Preset;
                selectedIndex = -1;
                selectedPrefab = null;
                LoadPrefabs();
            }
        }

        if (GUILayout.Toggle(currentMode == EditMode.Object, "오브젝트 모드", EditorStyles.toolbarButton))
        {
            if (currentMode != EditMode.Object)
            {
                currentMode = EditMode.Object;
                selectedIndex = -1;
                selectedPrefab = null;
                LoadPrefabs();
            }
        }
        GUILayout.EndHorizontal();
    }

    private void DrawObjectPlacementOptions()
    {
        EditorGUILayout.Space();
        GUILayout.Label("오브젝트 배치 설정", EditorStyles.boldLabel);

        selectedLane = EditorGUILayout.IntSlider("Lane", selectedLane, 0, 2);
        objectHeight = EditorGUILayout.FloatField("Y 위치", objectHeight);
        objectYRotation = EditorGUILayout.FloatField("Y축 회전", objectYRotation);
    }

    private void LoadPrefabs()
    {
        prefabNames.Clear();

        string searchPath = currentMode == EditMode.Preset
            ? "Assets/Resources/" + presetPath
            : "Assets/Resources/" + objectPath;

        var guids = AssetDatabase.FindAssets("t:Prefab", new[] { searchPath });

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null)
                prefabNames.Add(prefab.name);
        }
    }

    private void SelectPrefab(int index)
    {
        selectedIndex = index;
        string path = currentMode == EditMode.Preset ? presetPath : objectPath;
        string name = prefabNames[index];

        selectedPrefab = Resources.Load<GameObject>($"{path}/{name}");

        if (selectedPrefab == null)
        {
            Debug.LogError("프리팹을 Resources에서 불러오지 못했습니다");
        }
    }

    private void PlacePreset()
    {
        if (selectedPrefab == null)
        {
            Debug.LogWarning("선택된 프리셋이 없습니다");
            return;
        }

        if (mapRoot == null)
        {
            GameObject rootObj = GameObject.Find("MapRoot");
            if (rootObj == null)
                rootObj = new GameObject("MapRoot");
            mapRoot = rootObj.transform;
        }

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
        instance.transform.SetParent(mapRoot);

        if (mapRoot.childCount > 1)
        {
            Transform last = mapRoot.GetChild(mapRoot.childCount - 2);
            Transform lastEnd = last.Find("End");
            Transform currentStart = instance.transform.Find("Start");

            if (lastEnd != null && currentStart != null)
            {
                Quaternion fromTo = Quaternion.FromToRotation(currentStart.forward, lastEnd.forward);
                instance.transform.rotation = fromTo * instance.transform.rotation;

                Vector3 offset = currentStart.position - instance.transform.position;
                instance.transform.position = lastEnd.position - offset;
            }
            else
            {
                Debug.LogWarning("Start 또는 End 오브젝트가 누락되어 있습니다");
            }
        }
        else
        {
            instance.transform.position = Vector3.zero;
            instance.transform.rotation = Quaternion.identity;
        }

        Undo.RegisterCreatedObjectUndo(instance, "프리셋 배치");
    }

    private void PlaceMapObject()
    {
        if (selectedPrefab == null)
        {
            Debug.LogWarning("선택된 오브젝트가 없습니다");
            return;
        }

        float laneX = selectedLane; // x 좌표: lane 기준
        Vector3 position = new Vector3(laneX, objectHeight, SceneView.lastActiveSceneView.pivot.z);

        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
        instance.transform.position = position;
        instance.transform.rotation = Quaternion.Euler(0, objectYRotation, 0);

        Undo.RegisterCreatedObjectUndo(instance, "오브젝트 배치");
    }
}
