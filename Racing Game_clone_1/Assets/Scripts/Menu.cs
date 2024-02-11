using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Menu : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    public GameObject menuObj;
    public GameObject lobbyObj;
    public GameObject lobbyBackButton;
    // public Button MultiplayerButton;

    [Networked, OnChangedRender(nameof(CounterBoolChange))]
    private bool _NetworkedStartCountdown {get; set;}
    private bool _startCountdown;
    public GameObject CountdownObj;

    [Networked, OnChangedRender(nameof(CountChange))]
    public int _NetworkedPlayerRefs {get; set;}
    private int _playerCount;
    public TextMeshProUGUI countText;

    [Networked, OnChangedRender(nameof(CountdownChange))]
    private float _NetworkedCountdown {get; set;} = 15;
    private float _countdown;
    public TextMeshProUGUI countdownText;

    // private NetworkRunner _runner;
    private bool LoadInvoked;

    private void Awake() {
        // MultiplayerButton.onClick.AddListener(() => ConnectToRunner(GameMode.Shared));
    }

    private void Start() {
        Cursor.lockState = CursorLockMode.None;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Update() {
        if(_playerCount == 2 && HasStateAuthority)
        {
            // lobbyBackButton.SetActive(false);
            RpcToggleCounter(true);
            RpcDoCountdown(true);
        }
        else if(_playerCount > 0 && _playerCount < 14.9 && HasStateAuthority)
        {
            // lobbyBackButton.SetActive(true);
            RpcToggleCounter(false);
            RpcDoCountdown(false);
        }

        CountdownObj.SetActive(_startCountdown);
        countText.text = $"{_playerCount}/2";
        countdownText.text = $"{Math.Floor(_countdown)}";
    }

    public override void FixedUpdateNetwork()
    {
        // Debug.Log(_NetworkedPlayerRefs);
        // CountdownObj.SetActive(_NetworkedStartCountdown);
        if(_playerCount == 2 && HasStateAuthority)
        {
            // lobbyBackButton.SetActive(false);
            // RpcToggleCounter(true);
            RpcDoCountdown(true);
        }
        else if(_playerCount > 0 && _playerCount < 14.9 && HasStateAuthority)
        {
            // lobbyBackButton.SetActive(true);
            // RpcToggleCounter(false);
            RpcDoCountdown(false);
        }

        if(Runner.IsSceneAuthority && _NetworkedCountdown <= 0 && !LoadInvoked)
        {
            // if(!SceneManager.GetSceneByBuildIndex(1).isLoaded) Runner.LoadScene(SceneRef.FromIndex(1), LoadSceneMode.Additive);
            // if(SceneManager.GetSceneByBuildIndex(0).isLoaded) Runner.UnloadScene(SceneRef.FromIndex(0));
            RpcLoadGameScene();
            LoadInvoked = true;
        }
        else if(Runner.IsSceneAuthority && _NetworkedCountdown > 0 && LoadInvoked)
        {
            LoadInvoked = false;
        }
    }


    private void RpcLoadGameScene() {
        if(!SceneManager.GetSceneByBuildIndex(1).isLoaded) Runner.LoadScene(SceneRef.FromIndex(1), LoadSceneMode.Additive);
        if(SceneManager.GetSceneByBuildIndex(0).isLoaded) Runner.UnloadScene(SceneRef.FromIndex(0));
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LobbyMenu()
    {
        menuObj.SetActive(false);
        lobbyObj.SetActive(true);
    }

    public void LobbyBack()
    {
        menuObj.SetActive(true);
        lobbyObj.SetActive(false);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcToggleCounter(bool state)
    {
        _NetworkedStartCountdown = state;
        lobbyBackButton.SetActive(!state);
    }

    public void CounterBoolChange()
    {
        _startCountdown = _NetworkedStartCountdown;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcAddPlayerCount(PlayerRef playerRef)
    {
        _NetworkedPlayerRefs++;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcReducePlayerCount(PlayerRef playerRef)
    {
        _NetworkedPlayerRefs--;
    }

    public void CountChange()
    {
        _playerCount = _NetworkedPlayerRefs;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcDoCountdown(bool state)
    {
        if(_NetworkedCountdown <= 0)
        {
            _NetworkedCountdown = 0;
        }

        if(state && _NetworkedCountdown > 0)
        {
            _NetworkedCountdown -= 1 * Time.deltaTime;
        }
        else if(!state && _NetworkedCountdown < 15)
        {
            _NetworkedCountdown = 15;
        }
    }

    public void CountdownChange()
    {
        _countdown = _NetworkedCountdown;
    }

    public void PlayerJoined(PlayerRef player)
    {
        // throw new System.NotImplementedException();
        RpcAddPlayerCount(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        RpcReducePlayerCount(player);
    }
}