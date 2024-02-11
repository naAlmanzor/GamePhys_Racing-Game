using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
   [SerializeField] private Button MultiplayerButton;

    private NetworkRunner _runner;
   // public string RoomName => roomName.text;

    private void Awake()
    {
        MultiplayerButton.onClick.AddListener(() => ConnectToRunner(GameMode.Shared));
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
}
