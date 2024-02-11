using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameModeHandler : MonoBehaviour
{
   [SerializeField] private Button MultiplayerButton;

    private NetworkRunner _runner;
   // public string RoomName => roomName.text;

    private void Awake()
    {
        MultiplayerButton?.onClick.AddListener(() => ConnectToRunner(GameMode.Shared));
    }

    private void Update() {
        if(SceneManager.GetSceneByBuildIndex(0).isLoaded)
        {
            if(!MultiplayerButton)
            {
                MultiplayerButton = GameObject.FindGameObjectWithTag("MultButton").GetComponent<Button>();
                if(MultiplayerButton) MultiplayerButton?.onClick.AddListener(() => ConnectToRunner(GameMode.Shared));
            }
            
        }
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

        //  Debug.Log($"Hosting");
    }

    public async Task DisconnectFromRunner()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            await _runner.Shutdown();
        }
    }
}
