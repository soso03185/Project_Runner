using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 게임씬을 생성 및 초기화 등 관리해주는 클래스
/// </summary>
/// 
public class GameScene : Singleton<GameScene>
{
    public PlayerController m_Player;
    Camera mainCam;

    void Start()
    {
        Init();
        StartGameScene();
        // StartLobbyScene();
    }
    
    void Init()
    {
        // Managers //
        ResourceManager ResourceMgr = new GameObject().GetOrAddComponent<ResourceManager>();
        ResourceMgr.name = "@ResourceManager";

        // Player //
        m_Player = ResourceManager.Instance.InstantiatePrefab("Player").GetComponent<PlayerController>();
        
        // Camera //
        mainCam = Camera.main;
        CameraController myCam = mainCam.transform.GetComponent<CameraController>();
        myCam.m_Target = m_Player.transform;
        myCam.Init();
    }

    void StartGameScene()
    {
        // Managers //
        ResourceManager.Instance.InstantiatePrefab("Managers/@MapGenerator");

        // UI //
        Canvas uiCanvas = ResourceManager.Instance.InstantiatePrefab("UI/UI_GameScene").GetComponent<Canvas>();
        // uiCanvas.worldCamera = mainCam;

        // Map //
        MapGenerator.Instance.Init();
    }

    void StartLobbyScene()
    {
        // UI //
        Canvas uiCanvas = ResourceManager.Instance.InstantiatePrefab("UI/UI_LobbyScene").GetComponent<Canvas>();
        // uiCanvas.worldCamera = mainCam;
    }
}
