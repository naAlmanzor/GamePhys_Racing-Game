using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
  // [SerializeField] private TMP_InputField roomName;
   [SerializeField] private Button CreateButton;
   [SerializeField] private Button JoinButton;
   [SerializeField] private TextMeshProUGUI numberOfPlayers;
   [SerializeField] private TextMeshProUGUI clientNumberOfPlayers;
    
    public Button StartButton;
    public GameObject MenuPanel;
    public GameObject LobbyPanel;
    public GameObject ClientPlayerCountUI;

    public NetworkRunner _runner;

    public string NumberOfPlayers
    {
        get {return numberOfPlayers.text;}
        set {numberOfPlayers.text=value;}
    }
    public string ClientNumberOfPlayers
    {
        get {return clientNumberOfPlayers.text;}
        set {clientNumberOfPlayers.text=value;}
    }
   // public string RoomName => roomName.text;

    private void Awake()
    {
        CreateButton.onClick.AddListener(() => ConnectToRunner(GameMode.Shared)); // From GameHode.Host and GameMode.Client
        JoinButton.onClick.AddListener(() => ConnectToRunner(GameMode.Shared));
    }

    public async void ConnectToRunner(GameMode mode)
    {
        _runner = gameObject.AddComponent<NetworkRunner>();
       // _runner.ProvideInput = true;
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        await _runner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
          //  SessionName = UI.RoomName,
            //PlayerCount = 2,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        });

         Debug.Log($"Hosting");
    }

    // public void CreatePlayers()
    // {
    //     foreach(var player in _runner.ActivePlayers)
    //     {
    //         //// Create a unique position for the player
    //         Vector3 spawnPosition = new Vector3((player.RawEncoded % _runner.Config.Simulation.PlayerCount) * 3, 1, 0);
    //         NetworkObject networkPlayerObject = _runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
    //         // Keep track of the player avatars for easy access
    //         _spawnedCharacters.Add(player, networkPlayerObject);
    //     }

    //     LobbyPanel.SetActive(false);
    // }
}
